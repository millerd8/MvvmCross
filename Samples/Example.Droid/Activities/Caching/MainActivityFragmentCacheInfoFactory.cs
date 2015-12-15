using System;
using System.Collections.Generic;
using System.Linq;
using Example.Core.ViewModels;
using Example.Droid.Fragments;
using MvvmCross.Droid.Support.V7.Fragging;
using MvvmCross.Droid.Support.V7.Fragging.Caching;

namespace Example.Droid.Activities.Caching
{
    internal class MainActivityFragmentCacheInfoFactory : MvxCachedFragmentInfoFactory
    {
        private static readonly Dictionary<string, CustomFragmentInfo> MyFragmentsInfo = new Dictionary
            <string, CustomFragmentInfo>
        {
            {
                typeof (MenuViewModel).Name,
                new CustomFragmentInfo(typeof (MenuViewModel).Name, typeof (MenuFragment), typeof (MenuViewModel))
            },
            {
                typeof (HomeViewModel).Name,
                new CustomFragmentInfo(typeof (HomeViewModel).Name, typeof (HomeFragment), typeof (HomeViewModel),
                    isRoot: true)
            },
            {
                typeof (ExampleViewPagerViewModel).Name,
                new CustomFragmentInfo(typeof (ExampleViewPagerViewModel).Name, typeof (ExampleViewPagerFragment),
                    typeof (ExampleViewPagerViewModel), isRoot: true)
            },
            {
                typeof (ExampleRecyclerViewModel).Name,
                new CustomFragmentInfo(typeof (ExampleRecyclerViewModel).Name, typeof (ExampleRecyclerViewFragment),
                    typeof (ExampleRecyclerViewModel), isRoot: true)
            },
            {
                typeof (SettingsViewModel).Name,
                new CustomFragmentInfo(typeof (SettingsViewModel).Name, typeof (SettingsFragment),
                    typeof (SettingsViewModel), isRoot: true)
            }
        };

        public Dictionary<string, CustomFragmentInfo> GetFragmentsRegistrationData()
        {
            return MyFragmentsInfo;
        }

        public override IMvxCachedFragmentInfo CreateFragmentInfo(string tag, Type fragmentType, Type viewModelType,
            bool addToBackstack = false)
        {
            var fragInfo = MyFragmentsInfo[tag];
            return fragInfo;
        }

        public override SerializableMvxCachedFragmentInfo GetSerializableFragmentInfo(
            IMvxCachedFragmentInfo objectToSerialize)
        {
            var baseSerializableCachedFragmentInfo = base.GetSerializableFragmentInfo(objectToSerialize);
            var customFragmentInfo = objectToSerialize as CustomFragmentInfo;

            return new SerializableCustomFragmentInfo(baseSerializableCachedFragmentInfo)
            {
                IsRoot = customFragmentInfo.IsRoot
            };
        }

        public override IMvxCachedFragmentInfo ConvertSerializableFragmentInfo(
            SerializableMvxCachedFragmentInfo fromSerializableMvxCachedFragmentInfo)
        {
            var serializableCustomFragmentInfo = fromSerializableMvxCachedFragmentInfo as SerializableCustomFragmentInfo;
            var baseCachedFragmentInfo = base.ConvertSerializableFragmentInfo(fromSerializableMvxCachedFragmentInfo);

            return new CustomFragmentInfo(baseCachedFragmentInfo.Tag, baseCachedFragmentInfo.FragmentType,
                baseCachedFragmentInfo.ViewModelType, baseCachedFragmentInfo.AddToBackStack,
                serializableCustomFragmentInfo.IsRoot)
            {
                ContentId = baseCachedFragmentInfo.ContentId,
                CachedFragment = baseCachedFragmentInfo.CachedFragment
            };
        }

        internal class SerializableCustomFragmentInfo : SerializableMvxCachedFragmentInfo
        {
            public SerializableCustomFragmentInfo()
            {
                
            }

            public SerializableCustomFragmentInfo(SerializableMvxCachedFragmentInfo baseFragmentInfo)
            {
                AddToBackStack = baseFragmentInfo.AddToBackStack;
                ContentId = baseFragmentInfo.ContentId;
                FragmentType = baseFragmentInfo.FragmentType;
                Tag = baseFragmentInfo.Tag;
                ViewModelType = baseFragmentInfo.ViewModelType;
            }

            public bool IsRoot { get; set; }
        }
    }
}