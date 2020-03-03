using System.Collections.Generic;

public class WeightedRandom<T>
{
    private struct Value
    {
        public T value;

        public float probability;

        public Value(T value, float probability)
        {
            this.value = value;
            this.probability = probability;
        }
    }

    private System.Random _random;

    private List<Value> _weightedValues;

    private float _sumOfProbabilities = 0f;

    public WeightedRandom(int seed) : this(new System.Random(seed)) { }

    public WeightedRandom() : this(new System.Random()) { }

    public WeightedRandom(System.Random random)
    {
        this._random = random;
        this._weightedValues = new List<Value>();
    }

    public WeightedRandom<T> Add(T value, float probability)
    {
        if (probability <= 0f) return this;
        
        this._weightedValues.Add(new Value(value, probability));
        this._sumOfProbabilities += probability;

        return this;
    }

    public float sumOfProbabilities => this._sumOfProbabilities;

    public T Next()
    {
        double p = this._random.NextDouble() * this._sumOfProbabilities;

        foreach (var v in this._weightedValues)
        {
            p -= v.probability;
            if (p <= 0) return v.value;
        }

        return default(T);
    }

}