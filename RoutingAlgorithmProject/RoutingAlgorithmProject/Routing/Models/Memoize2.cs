using System;
using System.Collections.Generic;

namespace RoutingAlgorithmProject.Routing.Models
{
    public class Memoize2<P1, P2, R>
    {
        public Memoize2(Func<P1, P2, R> evaluator)
        {
            this.evaluator = evaluator ?? throw new ArgumentException("Evaluator may not be null");
        }

        public R Get(P1 parameter1, P2 parameter2)
        {
            KeyValuePair<P1, P2> pair = new KeyValuePair<P1, P2>(parameter1, parameter2);

            if (values.ContainsKey(pair))
            {
                return values[pair];
            }
            else
            {
                R value = evaluator.Invoke(parameter1, parameter2);
                values.Add(pair, value);
                return value;
            }
        }

        private Dictionary<KeyValuePair<P1, P2>, R> values = new Dictionary<KeyValuePair<P1, P2>, R>();   // TODO maybe use a hash as a key
        private Func<P1, P2, R> evaluator;
    }

}
