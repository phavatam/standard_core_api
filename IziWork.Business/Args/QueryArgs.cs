using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class QueryArgs
    {
        public string Predicate { get; set; }
        public object[] PredicateParameters { get; set; } // khong xac dinh duoc value kind
        public string Order { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public object Object { get; set; }
        #region Init
        public object[] InitPredicateParameters
        {
            get
            {
                var returnValue = new List<object>();
                foreach (var parameter in PredicateParameters)
                {
                    switch (parameter)
                    {
                        case JsonElement jsonElement:
                            switch (jsonElement.ValueKind)
                            {
                                case JsonValueKind.String:
                                    returnValue.Add(jsonElement.GetString());
                                    break;
                                case JsonValueKind.Number:
                                    returnValue.Add(jsonElement.GetInt32());
                                    break;
                                case JsonValueKind.True:
                                case JsonValueKind.False:
                                    returnValue.Add(jsonElement.GetBoolean());
                                    break;
                                default:
                                    returnValue.Add(jsonElement.ToString());
                                    break;
                            }
                            break;
                        default:
                            returnValue.Add(parameter);
                            break;
                    }
                }
                PredicateParameters = returnValue.ToArray();
                return PredicateParameters;
            }
        }
        #endregion
    }
}
