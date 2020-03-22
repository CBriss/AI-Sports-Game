var boatGame;
updateSpeedupValue();

function download(content, fileName, contentType) {
  var a = document.createElement("a");
  var file = new Blob([content], { type: contentType });
  a.href = URL.createObjectURL(file);
  a.download = fileName;
  a.click();
}

function drawCanvas() {
  canvas = document.createElement("canvas");
  canvas.id = "game-canvas";
  canvas.width = 500;
  canvas.height = 600;
  context = canvas.getContext("2d");
  document.getElementById("game-container").appendChild(canvas);
  return context;
}

function outputBestBoat() {
  let inputWeights = boatGame.geneticAlgorithm.bestBoat.brain.input_weights;
  let outputWeights = boatGame.geneticAlgorithm.bestBoat.brain.output_weights;
  let jsonData = JSON.stringify({
    mode: boatGame.mode,
    inputWeights: inputWeights.arraySync(),
    outputWeights: outputWeights.arraySync()
  });
  
  download(jsonData, "currentBestBoat_brain.txt", "text/plain");
}

function randn_bm() {
  var u = 0,
    v = 0;
  while (u === 0) u = Math.random(); //Converting [0,1) to (0,1)
  while (v === 0) v = Math.random();
  return Math.sqrt(-1.2 * Math.log(u)) * Math.cos(1.2 * Math.PI * v);
}

function showMenu() {
  // Kill the canvas
  var canvas = document.getElementById("game-canvas");
  document.getElementById("game-container").removeChild(canvas);

  // Show the menu
  var menu = document.getElementById("menu-container");
  menu.style.display = "block";
}

function startGame(playerFlag, mode, seed_input_weights=null, seed_output_weights=null) {
  // Hide the menu
  var menu = document.getElementById("menu-container");
  menu.style.display = "none";

  // Start the canvas
  boatGame = new BoatGame(playerFlag, mode, seed_input_weights, seed_output_weights);
}

function updateSpeedupValue() {
  var slider = document.getElementById("speedup-slider");
  var output = document.getElementById("speedup-value");
  slider.oninput = function() {
    output.innerHTML = this.value;
    if (boatGame) {
      boatGame.speedMode = this.value;
    }
  };
}

function uploadBoat(files) {
  let file = files[0];
  const reader = new FileReader();
  reader.readAsText(file);
  reader.onload = function(event) {
    let json_string = JSON.parse(event.target.result);
    let mode = json_string['mode'];
    let input_weights = json_string['inputWeights'];
    let output_weights = json_string['outputWeights'];
    console.log(input_weights);
    console.log(output_weights);
    startGame(false, mode, [input_weights, output_weights]);
  };
}
