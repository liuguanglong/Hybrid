using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Hybrid.Web.Shared;

namespace Hybrid.Web.UIComponent
{
    public class BcValidationRule
    {
        public String Script { get; set; }
        public List<BcValidationRuleParam> listParams { get; set; }

        public bool TryValidate(Dictionary<String, Object> parameters)
        {
            List<ParameterExpression> listparams = new List<ParameterExpression>();
            List<Object> args = new List<Object>();
            foreach (var param in listParams)
            {
                listparams.Add(Expression.Parameter(param.DataType, param.Name));
                args.Add(parameters[param.Name]);
            }

            var e = DynamicExpressionParser.ParseLambda(listparams.ToArray(),typeof(Boolean), Script);

            var Lambda = e.Compile();
            return (Boolean)Lambda.DynamicInvoke(args.ToArray());
        }
    }
}
