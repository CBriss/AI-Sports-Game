class NeuralNetwork {
  constructor(input_nodes, hidden_nodes, output_nodes, preset_weights) {
    this.input_nodes = input_nodes;
    this.hidden_nodes = hidden_nodes;
    this.output_nodes = output_nodes;

    //Initialize Random Weights
    if(preset_weights){
      this.input_weights = tf.tensor(preset_weights[0]);
      this.output_weights = tf.tensor(preset_weights[1]);
    }
    else{
      this.input_weights = tf.randomNormal([this.input_nodes, this.hidden_nodes]);
      this.output_weights = tf.randomNormal([
        this.hidden_nodes,
        this.output_nodes
    ]);
    }
  }

  predict(user_input) {
    let output;
    tf.tidy(() => {
      let input_layer = tf.tensor(user_input, [1, this.input_nodes]);
      let hidden_layer = input_layer.matMul(this.input_weights).logSigmoid();
      let output_layer = hidden_layer.matMul(this.output_weights).logSigmoid();
      // output = output_layer.softmax().dataSync();
      output = output_layer.softmax().dataSync();
    });
    return output;
  }

  clone() {
    let clonie = new NeuralNetwork(
      this.input_nodes,
      this.hidden_nodes,
      this.output_nodes
    );
    clonie.dispose();
    clonie.input_weights = tf.clone(this.input_weights);
    clonie.output_weights = tf.clone(this.output_weights);
    return clonie;
  }

  dispose() {
    this.input_weights.dispose();
    this.output_weights.dispose();
  }
}
