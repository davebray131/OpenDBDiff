using System;

namespace OpenDBDiff.Abstractions.Schema.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class SchemaNodeAttribute : Attribute
    {
        public SchemaNodeAttribute(string name)
        {
            this.Name = name;
            this.Image = "Folder";
        }

        public SchemaNodeAttribute(string name, string image)
        {
            this.Name = name;
            this.Image = image;
        }

        public SchemaNodeAttribute(string name, string image, bool isFullName)
        {
            this.Name = name;
            this.Image = image;
            this.IsFullName = isFullName;
        }

        public string Name { get; private set; }

        public string Image { get; private set; }

        public bool IsFullName { get; private set; }
    }
}
