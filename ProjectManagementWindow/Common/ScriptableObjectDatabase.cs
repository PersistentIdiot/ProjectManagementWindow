using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace ProjectManagementWindow.Common
{
    public abstract class ScriptableObjectDatabase<TData> : ScriptableObject where TData : Object
    {
        public const string DatabaseName = nameof(TData) + DatabaseSuffix;


        // This interface needs to be attached to T if you the DB to index elements. Not required.
        public interface IDatabaseKey
        {
            object GetKey();
        }

        public const string DatabaseSuffix = "Database";


        public static string GetDatabaseName() => typeof(TData).Name + DatabaseSuffix;

        // @Jon: we use the list only for seeding the backing dictionaries
        [FormerlySerializedAs("Database")]
        [SerializeField]
        private List<TData> database;

        [SerializeField]
        private bool debug = true;

        private Dictionary<object, TData> _keyIndex = new Dictionary<object, TData>();
        private Dictionary<Type, TData> _typeIndex = new Dictionary<Type, TData>();
        private Dictionary<string, TData> _nameIndex = new Dictionary<string, TData>();

        private static ScriptableObjectDatabase<TData> _instance;

        [NotNull]
        public static ScriptableObjectDatabase<TData> Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = LoadFromResources(GetDatabaseName());
                    _instance.Init();
                }

                return _instance;
            }
        }


        private void Init()
        {
            // @Jon: we seed the dictionaries from the serialized list

            _keyIndex = database.Where(it => it != null)
                .ToDictionary(it => it is IDatabaseKey databaseKey ? databaseKey.GetKey() : it);

            _typeIndex = database.Where(it => it != null).ToDictionary(it => it.GetType());
            _nameIndex = database.Where(it => it != null).ToDictionary(it => it.name);
            Debug.Log(
                $"{nameof(ScriptableObjectDatabase<TData>)} : Indices initialized with {_keyIndex.Count} entries");
        }

        private static ScriptableObjectDatabase<TData> LoadFromResources(string resourceName)
        {
            ScriptableObjectDatabase<TData> instance = Resources.Load(resourceName) as ScriptableObjectDatabase<TData>;
            if (instance == null)
                throw new Exception(
                    $"{typeof(ScriptableObjectDatabase<TData>).Name} : Failed to load ScriptableObjectDatabase<{typeof(TData).Name}>! Check Path ({DatabaseName})");

            return instance;
        }


        // @Jon: added indexer
        public TData this[object key]
        {
            get
            {
                if (key is Type type)
                    return GetByType(type);
                else
                    return GetByKey(key);
            }
        }

        // @Jon: added API for dictionary

        public static bool ContainsKey(object key) => _instance._keyIndex.ContainsKey(key);
        public static bool ContainsName(string key) => _instance._nameIndex.ContainsKey(key);
        public static bool ContainsType(Type key) => _instance._typeIndex.ContainsKey(key);
        public static bool ContainsType<TItem>() => _instance._typeIndex.ContainsKey(typeof(TItem));

        public static TData GetByKey(object key) => _instance._keyIndex[key];
        public static TItem GetByKey<TItem>(object key) where TItem : TData => (TItem)_instance._keyIndex[key];

        public static TItem GetByType<TItem>() where TItem : TData => (TItem)_instance._typeIndex[typeof(TItem)];
        public static TData GetByType(Type key) => _instance._typeIndex[key];

        public static TData GetByName(string key) => _instance._nameIndex[key];
        public static TItem GetByName<TItem>(string key) where TItem : TData => (TItem)_instance._nameIndex[key];


        private void OnValidate()
        {
            // If debugging, make sure we can load it
            if (debug)
            {
                ScriptableObjectDatabase<TData> testLoad =
                    Resources.Load(DatabaseName) as ScriptableObjectDatabase<TData>;

                if (testLoad != null)
                    Debug.Log(
                        $"{nameof(ScriptableObjectDatabase<TData>)} : Loaded database for type {typeof(TData).Name}");
                else
                    Debug.LogError(
                        $"{nameof(ScriptableObjectDatabase<TData>)} : Failed to load database for type {typeof(TData).Name}; Check {DatabaseName} ");
            }

            Init();
        }
    }
}