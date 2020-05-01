using Services.Logging;
using System.Reflection;
using Unity;

namespace Server.Factory
{
    public sealed class ObjFactory
    {
        #region Fields

        private static readonly object _datalock = new object();
        private static ObjFactory _instance { get; set; }
        private IUnityContainer _objContainer { get; set; }

        #endregion Fields

        #region Instance

        public static ObjFactory Instance
        {
            get
            {
                lock (_datalock)
                {
                    if (_instance == null)
                    {
                        _instance = new ObjFactory();
                    }
                    return _instance;
                }
            }
        }

        #endregion Instance

        #region Constructor

        private ObjFactory()
        {
            _objContainer = new UnityContainer();
        }

        #endregion Constructor

        #region Logs

        internal ILog CreateLogger()
        {
            if (!_objContainer.IsRegistered(typeof(Logger), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterSingleton(typeof(Logger), MethodBase.GetCurrentMethod().Name);
            }
            return (Logger)_objContainer.Resolve(typeof(Logger), MethodBase.GetCurrentMethod().Name);
        }

        #endregion Logs
    }
}