using System;
using System.IO;
using System.Windows;

namespace ActivityLogger
{
    public abstract class ListingWindowVM : DependencyObject
    {
        #region IsFolder Property Members

        public const string PropertyName_IsFolder = "IsFolder";

        public static readonly DependencyPropertyKey IsFolderPropertyKey =
            DependencyProperty.RegisterReadOnly(ListingWindowVM.PropertyName_IsFolder, typeof(bool), typeof(ListingWindowVM),
                new PropertyMetadata(true));

        public static readonly DependencyProperty IsFolderProperty =
          ListingWindowVM.IsFolderPropertyKey.DependencyProperty;

        public bool IsFolder
        {
            get { return (bool)(this.GetValue(ListingWindowVM.IsFolderProperty)); }
            private set { this.SetValue(ListingWindowVM.IsFolderPropertyKey, value); }
        }

        #endregion

        #region FullPath Property Members

        public const string PropertyName_FullPath = "FullPath";

        public static readonly DependencyPropertyKey FullPathPropertyKey =
            DependencyProperty.RegisterReadOnly(ListingWindowVM.PropertyName_FullPath, typeof(string), typeof(ListingWindowVM),
                new PropertyMetadata(""));

        public static readonly DependencyProperty FullPathProperty =
          ListingWindowVM.FullPathPropertyKey.DependencyProperty;

        public string FullPath
        {
            get { return this.GetValue(ListingWindowVM.FullPathProperty) as string; }
            private set { this.SetValue(ListingWindowVM.FullPathPropertyKey, value); }
        }

        #endregion

        #region Name Property Members

        public const string PropertyName_Name = "Name";

        public static readonly DependencyPropertyKey NamePropertyKey =
            DependencyProperty.RegisterReadOnly(ListingWindowVM.PropertyName_Name, typeof(string), typeof(ListingWindowVM),
                new PropertyMetadata(""));

        public static readonly DependencyProperty NameProperty =
          ListingWindowVM.NamePropertyKey.DependencyProperty;

        public string Name
        {
            get { return this.GetValue(ListingWindowVM.NameProperty) as string; }
            private set { this.SetValue(ListingWindowVM.NamePropertyKey, value); }
        }

        #endregion

        #region Extension Property Members

        public const string PropertyName_Extension = "Extension";

        public static readonly DependencyPropertyKey ExtensionPropertyKey =
            DependencyProperty.RegisterReadOnly(ListingWindowVM.PropertyName_Extension, typeof(string), typeof(ListingWindowVM),
                new PropertyMetadata(""));

        public static readonly DependencyProperty ExtensionProperty =
          ListingWindowVM.ExtensionPropertyKey.DependencyProperty;

        public string Extension
        {
            get { return this.GetValue(ListingWindowVM.ExtensionProperty) as string; }
            private set { this.SetValue(ListingWindowVM.ExtensionPropertyKey, value); }
        }

        #endregion

        #region CreationTime Property Members

        public const string PropertyName_CreationTime = "CreationTime";

        public static readonly DependencyPropertyKey CreationTimePropertyKey =
            DependencyProperty.RegisterReadOnly(ListingWindowVM.PropertyName_CreationTime, typeof(DateTime), typeof(ListingWindowVM),
                new PropertyMetadata(DateTime.MinValue));

        public static readonly DependencyProperty CreationTimeProperty =
          ListingWindowVM.CreationTimePropertyKey.DependencyProperty;

        public DateTime CreationTime
        {
            get { return (DateTime)(this.GetValue(ListingWindowVM.CreationTimeProperty)); }
            private set { this.SetValue(ListingWindowVM.CreationTimePropertyKey, value); }
        }

        #endregion

        #region LastWriteTime Property Members

        public const string PropertyName_LastWriteTime = "LastWriteTime";

        public static readonly DependencyPropertyKey LastWriteTimePropertyKey =
            DependencyProperty.RegisterReadOnly(ListingWindowVM.PropertyName_LastWriteTime, typeof(DateTime), typeof(ListingWindowVM),
                new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty LastWriteTimeProperty =
          ListingWindowVM.LastWriteTimePropertyKey.DependencyProperty;

        public DateTime LastWriteTime
        {
            get { return (DateTime)(this.GetValue(ListingWindowVM.LastWriteTimeProperty)); }
            private set { this.SetValue(ListingWindowVM.LastWriteTimePropertyKey, value); }
        }

        #endregion

        protected ListingWindowVM(bool isFolder)
        {
            this.IsFolder = isFolder;
        }

        protected ListingWindowVM(bool isFolder, FileSystemInfo fileSystemInfo)
            : this(isFolder)
        {
            this.Name = fileSystemInfo.Name;
            this.FullPath = fileSystemInfo.FullName;
            this.Extension = fileSystemInfo.Extension;
            this.CreationTime = fileSystemInfo.CreationTime;
            this.LastWriteTime = fileSystemInfo.LastWriteTime;
        }
    }
}