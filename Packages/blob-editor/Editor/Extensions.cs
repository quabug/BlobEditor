using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Unity.Entities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Blob.Editor
{
    public ref struct BuilderFactory
    {
        public Type BuilderType { get; }
        private readonly Func<object> _creator;

        public BuilderFactory([NotNull] Type builderType)
        {
            BuilderType = builderType;
            _creator = () => Activator.CreateInstance(builderType);
        }

        public BuilderFactory([NotNull] Type builderType, [NotNull] Func<object> creator)
        {
            BuilderType = builderType;
            _creator = creator;
        }

        public object Create() => _creator();
    }

    public static class Extensions
    {
        public static object GetSiblingValue(this SerializedProperty property, string name)
        {
            var obj = GetDeclaringObject(property);
            var type = obj.GetType();
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var fieldInfo = type.GetField(name, flags);
            if (fieldInfo != null) return fieldInfo.GetValue(obj);
            var propertyInfo = type.GetProperty(name, flags);
            if (propertyInfo != null) return propertyInfo.GetValue(obj);
            var methodInfo = type.GetMethod(name, flags);
            return methodInfo.Invoke(obj, Array.Empty<object>());
        }

        public static object GetSiblingFieldValue(this SerializedProperty property, string fieldName)
        {
            var obj = GetDeclaringObject(property);
            var fieldInfo = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return fieldInfo.GetValue(obj);
        }

        public static PropertyInfo GetSiblingPropertyInfo(this SerializedProperty property, string propertyName)
        {
            var obj = GetDeclaringObject(property);
            return obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public static MethodInfo GetSiblingMethodInfo(this SerializedProperty property, string methodName)
        {
            var obj = GetDeclaringObject(property);
            return obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public static object GetDeclaringObject(this SerializedProperty property)
        {
            return property.GetFieldsByPath().Reverse().Skip(1).First().field;
        }

        public static object GetObject(this SerializedProperty property)
        {
            return property.GetFieldsByPath().Last().field;
        }

        private static Regex _arrayData = new Regex(@"^data\[(\d+)\]$");

        public static IEnumerable<(object field, FieldInfo fi)> GetFieldsByPath(this SerializedProperty property)
        {
            var obj = (object)property.serializedObject.targetObject;
            FieldInfo fi = null;
            yield return (obj, fi);
            var pathList = property.propertyPath.Split('.');
            for (var i = 0; i < pathList.Length; i++)
            {
                var fieldName = pathList[i];
                if (fieldName == "Array" && i + 1 < pathList.Length && _arrayData.IsMatch(pathList[i + 1]))
                {
                    i++;
                    var itemIndex = int.Parse(_arrayData.Match(pathList[i]).Groups[1].Value);
                    var array = ((Array)obj);
                    obj = array != null && itemIndex < array.Length ? array.GetValue(itemIndex) : null;
                    yield return (obj, fi);
                }
                else
                {
                    var t = Field(obj, obj?.GetType() ?? fi.FieldType, fieldName);
                    obj = t.field;
                    fi = t.fi;
                    yield return t;
                }
            }

            (object field, FieldInfo fi) Field(object declaringObject, Type declaringType, string fieldName)
            {
                var fieldInfo = declaringType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                var fieldValue = declaringObject == null ? null : fieldInfo.GetValue(declaringObject);
                return (fieldValue, fieldInfo);
            }
        }

        internal static (Regex, string) ParseReplaceRegex(this string pattern, string separator = "||")
        {
            if (string.IsNullOrEmpty(pattern)) return (null, null);
            var patterns = pattern.Split(new[] { separator }, StringSplitOptions.None);
            if (patterns.Length == 2) return (new Regex(patterns[0]), patterns[1]);
            throw new ArgumentException($"invalid number of separator ({separator}) in pattern ({pattern})");
        }

        public static (object field, FieldInfo fieldInfo) GetTargetField(this SerializedProperty property)
        {
            return property.GetFieldsByPath().ElementAt(1);
        }

        public static (object field, FieldInfo fieldInfo) GetPropertyField(this SerializedProperty property)
        {
            return property.GetFieldsByPath().Last();
        }

        public static FieldInfo GetTargetFieldInfo(this SerializedProperty property)
        {
            return property.GetFieldsByPath().ElementAt(1).fi;
        }

        public static Type GetGenericType(this PropertyDrawer propertyDrawer)
        {
            return propertyDrawer.fieldInfo.DeclaringType.GetGenericType();
        }

        public static T GetCustomAttribute<T>(this SerializedProperty property) where T : Attribute
        {
            var (_, fieldInfo) = property.GetPropertyField();
            return fieldInfo.GetCustomAttribute<T>();
        }

        public static FieldInfo GetTargetFieldInfo(this SerializedProperty property, string fieldName)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return property.serializedObject.targetObject.GetType().GetField(fieldName, flags);
        }

        public static Type GetGenericType(this Type type)
        {
            while (type != null)
            {
                if (type.IsGenericType) return type.GenericTypeArguments.FirstOrDefault();
                type = type.BaseType;
            }

            return null;
        }

        public static Func<Rect, string, string[], int> PopupFunc(this SerializedProperty property)
        {
            return (position, label, options) =>
            {
                var optionIndex = Array.IndexOf(options, property.stringValue);
                if (optionIndex < 0) optionIndex = 0;
                optionIndex = EditorGUI.Popup(position, label, optionIndex, options);
                property.stringValue = optionIndex < options.Length ? options[optionIndex] : "";
                return optionIndex;
            };
        }

        public static IEnumerable<SerializedProperty> GetVisibleChildren(this SerializedProperty serializedProperty)
        {
            var iter = serializedProperty.Copy();
            var end = serializedProperty.GetEndProperty();
            iter.NextVisible(true);
            while (!SerializedProperty.EqualContents(iter, end))
            {
                yield return iter.Copy();
                iter.NextVisible(false);
            }
        }

        private static Func<SerializedProperty, Type, Type> _getDrawerTypeForPropertyAndType;

        public static Type GetDrawerTypeForPropertyAndType(this SerializedProperty property, Type type)
        {
            if (_getDrawerTypeForPropertyAndType == null)
            {
                var internalMethod = typeof(PropertyDrawer).Assembly
                        .GetType("UnityEditor.ScriptAttributeUtility")
                        .GetMethod("GetDrawerTypeForPropertyAndType", BindingFlags.Static | BindingFlags.NonPublic)
                    ;
                _getDrawerTypeForPropertyAndType = (Func<SerializedProperty, Type, Type>)internalMethod.CreateDelegate(typeof(Func<SerializedProperty, Type, Type>));
            }
            return _getDrawerTypeForPropertyAndType(property, type);
        }

        public static Type[] FindGenericArgumentsOf(this Type type, Type baseType)
        {
            Assert.IsTrue(baseType.IsGenericType);
            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == baseType)
                    return type.GenericTypeArguments;
                type = type.BaseType;
            }

            throw new ArgumentException();
        }

        public static SerializedProperty FindProperProperty(this SerializedProperty self)
        {
            var type = self?.GetObject()?.GetType();
            var customDrawer = type == null ? null : self.GetDrawerTypeForPropertyAndType(type);
            if (type != null && customDrawer == null)
            {
                var children = self.GetVisibleChildren().ToArray();
                if (children.Length == 1) return children[0];
            }

            return self;
        }

        public static IEnumerable<T> Yield<T>(this T value)
        {
            yield return value;
        }

        public static BuilderFactory FindBuilderCreator([NotNull] this FieldInfo fieldInfo)
        {
            var customBuilder = fieldInfo.GetCustomAttribute<CustomBuilderAttribute>()?.BuilderType;
            return GetBuilderFactory(fieldInfo.FieldType, customBuilder);
        }

        public static BuilderFactory GetBuilderFactory([NotNull] this Type valueType, Type customBuilder)
        {
            var builderType = typeof(Builder<>).MakeGenericType(valueType);
            var builders = TypeCache.GetTypesDerivedFrom(builderType);
            if (customBuilder == null && builders.Count == 1) return new BuilderFactory(builders[0]);
            if (customBuilder != null && builders.Contains(customBuilder)) return new BuilderFactory(customBuilder);
            if (customBuilder != null) throw new InvalidCustomBuilderException($"Invalid {customBuilder.Name} of {valueType.Name}, must be one of [{string.Join(",", builders.Select(b => b.Name))}]");

            try
            {
                var defaultBuilder = builders.SingleOrDefault(b => b.GetCustomAttribute<DefaultBuilderAttribute>() != null);
                return defaultBuilder == null ? DynamicBuilder() : new BuilderFactory(defaultBuilder);
            }
            catch (Exception ex)
            {
                throw new MultipleBuildersException($"There's multiple builders [{string.Join(",", builders.Select(b => b.Name))}] for {valueType.Name}, must mark one of them as `DefaultBuilder` or use `CustomBuilder` on this field", ex);
            }

            BuilderFactory DynamicBuilder()
            {
                var factory = DynamicBuilderFactoryRegister.FindFactory(valueType);
                if (factory == null) throw new ArgumentException($"cannot find any builder for {valueType}");
                return new BuilderFactory(factory.BuilderType, () => factory.Create(valueType));
            }
        }
    }

    [Serializable]
    public class InvalidCustomBuilderException : Exception
    {
        public InvalidCustomBuilderException() {}
        public InvalidCustomBuilderException(string message) : base(message) {}
        public InvalidCustomBuilderException(string message, Exception inner) : base(message, inner) {}
        protected InvalidCustomBuilderException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }

    [Serializable]
    public class MultipleBuildersException : Exception
    {
        public MultipleBuildersException() {}
        public MultipleBuildersException(string message) : base(message) {}
        public MultipleBuildersException(string message, Exception inner) : base(message, inner) {}
        protected MultipleBuildersException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}