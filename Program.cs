// See https://aka.ms/new-console-template for more information
Console.WriteLine("\n --------------- Hello, World! --------------- \n");


// Lets code a neural network

// We'll start with 3 neurons with 4 inputs

// Inputs are a list of numbers (one input per neuron)
var inputs = new List<float>() { 1, 2, 3, 2.5F };

// Weights are a a list of numbers (one set of weights per neuron)
var weights = new List<List<float>>() {
    new List<float>() { 0.2f, 0.8f, -0.5f, 1.0f },
    new List<float>() { 0.5f, -0.91f, 0.26f, -0.5f },
    new List<float>() { -0.26f, -0.27f, 0.17f, 0.87f }
};

// Bias is a number (one bias per neuron)
var biases = new List<float>() { 2.0f, 1.0f, -1.2f };

// Output is a number (one output per neuron)
var output = new List<float>() {
    inputs[0] * weights[0][0] +
    inputs[1] * weights[0][1] +
    inputs[2] * weights[0][2] +
    inputs[3] * weights[0][3] + biases[0],

    inputs[0] * weights[1][0] +
    inputs[1] * weights[1][1] +
    inputs[2] * weights[1][2] +
    inputs[3] * weights[1][3] + biases[1],

    inputs[0] * weights[2][0] +
    inputs[1] * weights[2][1] +
    inputs[2] * weights[2][2] +
    inputs[3] * weights[2][3] + biases[2]
};

// Let's begin to simplify the code

// Lets generate the output using a loop
output = new List<float>();

// We iterate through the list of weights because the neurons are
// represented by having a set weights
for (int i = 0; i < weights.Count; i++)
{
  var neuronOutput = 0.0f;
  // Iterate throught the inputs and multiply them by the weights
  for (int j = 0; j < inputs.Count; j++)
  {
    neuronOutput += inputs[j] * weights[i][j];
  }
  neuronOutput += biases[i];
  output.Add(neuronOutput);
}

Console.WriteLine("\n--------------- Output: ---------------\n");
Console.WriteLine($"{output[0]}, {output[1]}, {output[2]}");

// Let's make a function to calculate the dot product of a neuron
float Dot(List<float> a, List<float> b)
{
  var output = 0.0f;
  for (int i = 0; i < a.Count; i++)
  {
    output += a[i] * b[i];
  }
  return output;
}

// Let's make a function to calculate the dot product of multiple neurons
List<float> DotProduct(List<List<float>> a, List<float> b)
{
  var output = new List<float>();
  for (int i = 0; i < a.Count; i++)
  {
    output.Add(Dot(a[i], b));
  }
  return output;
}

// Let's make a function to add two lists of numbers at the same index together
List<float> Add(List<float> a, List<float> b)
{
  var output = new List<float>();
  for (int i = 0; i < a.Count; i++)
  {
    output.Add(a[i] + b[i]);
  }
  return output;
}

// Lets use the functions to calculate the output
output = Add(DotProduct(weights, inputs), biases);

Console.WriteLine("\n--------------- Output: ---------------\n");
Console.WriteLine($"{output[0]}, {output[1]}, {output[2]}");

// Let's create a Vector class to make the code more readable
// Let's use the new Vector class to calculate the output
var vectorInputs = new Vector(inputs.ToArray());
var vectorWeights = new List<Vector>() {
    new Vector(weights[0].ToArray()),
    new Vector(weights[1].ToArray()),
    new Vector(weights[2].ToArray())
};
var vectorBiases = new Vector(biases.ToArray());

var vectorOutput = Vector.Dot(vectorWeights, vectorInputs) + vectorBiases;

Console.WriteLine("\n--------------- Output: ---------------\n");
Console.WriteLine($"{vectorOutput.Values[0]}, {vectorOutput.Values[1]}, {vectorOutput.Values[2]}");

// Lets convert the input to be a batch of inputs.
// This results in a list of vectors or a matrix
var batch = new Vector[] {
    new Vector(new float[] { 1, 2, 3, 2.5f }),
    new Vector(new float[] { 2.0f, 5.0f, -1.0f, 2.0f }),
    new Vector(new float[] { -1.5f, 2.7f, 3.3f, -0.8f })
};

// Should we just use multidimensional arrays?
// var batch2 = new float[,] {
//     { 1, 2, 3, 2.5f },
//     { 2.0f, 5.0f, -1.0f, 2.0f },
//     { -1.5f, 2.7f, 3.3f, -0.8f }
// };

// Lets define a Matrix Dot product function
Vector[,] MatrixDot(Vector[,] a, Vector[,] b)
{
  var output = new Vector[a.GetLength(0), b.GetLength(1)];
  for (int i = 0; i < a.GetLength(0); i++)
  {
    for (int j = 0; j < b.GetLength(1); j++)
    {
      var row = new Vector();
      for (int k = 0; k < a.GetLength(1); k++)
      {
        row[k] = Vector.Dot(a[i, k], b[k, j]);
      }
      output[i, j] = row;
    }
  }
  return output;
}

Console.WriteLine("\n-------------------- END -------------------\n");

/// <summary>
/// Lets define a Vector class to make the code more readable
/// and use operator overloading to make the code more concise
/// </summary>
public class Vector
{
  public float[] Values { get; set; }
  public int Count { get { return Values.Length; } }

  public Vector(float[] values)
  {
    Values = values;
  }

  public Vector()
  {
    Values = Array.Empty<float>();
  }

  /// <summary>
  /// Indexer to make it easy to access the values in the vector
  /// </summary>
  public float this[int i]
  {
    get { return Values[i]; }
    set { Values[i] = value; }
  }

  /// <summary>
  /// Add two vectors together by taking the sum
  /// of the values at the same index
  /// </summary>
  public static Vector operator +(Vector a, Vector b)
  {
    var output = new float[a.Count];
    for (int i = 0; i < a.Count; i++)
    {
      output[i] = a[i] + b[i];
    }
    return new Vector(output);
  }

  /// <summary>
  /// Subtract two vectors together by taking the difference
  /// of the values at the same index
  /// </summary>
  public static Vector operator -(Vector a, Vector b)
  {
    var output = new float[a.Count];
    for (int i = 0; i < a.Count; i++)
    {
      output[i] = a.Values[i] - b.Values[i];
    }
    return new Vector(output);
  }

  /// <summary>
  /// Multiply a vector by a scalar by multiplying
  /// each value in the vector by the scalar
  /// </summary>
  public static Vector operator *(Vector a, float b)
  {
    var output = new float[a.Count];
    for (int i = 0; i < a.Count; i++)
    {
      output[i] = a.Values[i] * b;
    }
    return new Vector(output);
  }

  /// <summary>
  /// Take the dot product of each vector in a list
  /// with the same vector and return a list of the results
  /// </summary>
  public static Vector Dot(List<Vector> a, Vector b)
  {
    var output = new float[a.Count];
    for (int i = 0; i < a.Count; i++)
    {
      output[i] = Dot(a[i], b);
    }
    return new Vector(output);
  }

  /// <summary>
  /// Take the dot product of two vectors
  /// </summary>
  public static float Dot(Vector a, Vector b)
  {
    var output = 0.0f;
    for (int i = 0; i < a.Count; i++)
    {
      output += a.Values[i] * b.Values[i];
    }
    return output;
  }
}
