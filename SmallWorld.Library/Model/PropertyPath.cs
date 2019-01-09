using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SmallWorld.Library.Model
{
    public class PropertyPath<TRoot>
    {
        public IReadOnlyList<PropertyInfo> Properties { get; }

        public Type PropertyType;

        private PropertyPath(IReadOnlyList<PropertyInfo> tree)
        {
            Properties = tree;

            PropertyType = Properties.Last().PropertyType;
        }

        public override string ToString()
        {
            return $"{PropertyType.Name} {string.Join(".", Properties.Select(p => p.Name))}";
        }

        public bool TrySetValue(TRoot root, object value)
        {
            object node = root;

            foreach (var prop in Properties.Take(Properties.Count - 1))
            {
                node = prop.GetValue(node);
                if (node == null)
                    return false;
            }

            Properties.Last().SetValue(node, value);
            return true;
        }

        public object GetValue(TRoot root)
        {
            object value = root;

            foreach (var prop in Properties)
            {
                if (value == null) throw new NullReferenceException();
                value = prop.GetValue(value);
            }

            return value;
        }

        public static PropertyPath<TRoot> Parse(string expr)
        {
            var node = typeof(TRoot);
            var tree = new List<PropertyInfo>();

            foreach (var symbol in expr.Split('.'))
            {
                var prop = node.GetProperty(symbol);
                if (prop == null)
                    throw new ArgumentException("Invalid expression: " + expr);

                tree.Add(prop);
                node = prop.PropertyType;
            }

            return new PropertyPath<TRoot>(tree);
        }

        public static PropertyPath<TRoot> Create<TField>(Expression<Func<TRoot, TField>> expression)
        {
            var node = expression.Body;
            var tree = new List<PropertyInfo>();

            while (node is MemberExpression member)
            {
                tree.Insert(0, (PropertyInfo)member.Member);

                node = member.Expression;
            }

            if (!(node is ParameterExpression))
                throw new ArgumentException("Invalid expression: " + expression);

            return new PropertyPath<TRoot>(tree);
        }
    }
}
