class Boat extends GameComponent {
  constructor(ctx, playerFlag, yAxisMovement, seed_weights=null) {
    super();

    let possibleColors = ["Blue", "Green", "Pink", "Purple", "Red", "Yellow"];
    let randomColor =
      possibleColors[Math.floor(Math.random() * possibleColors.length)];
    this.setComponentValues(
      Math.random() * (ctx.canvas.width - 25),
      Math.random() * (ctx.canvas.height - 125),
      25,
      100,
      "images/boat" + randomColor + ".png"
    );

    this.person = new Person(ctx, this.x + this.width / 2, this.y);
    if(yAxisMovement)
      this.brain = new NeuralNetwork(5, 50, 4, seed_weights);
    else 
      this.brain = new NeuralNetwork(4, 5, 2, seed_weights);

    this.score = 0;
    this.distanceTraveled = 0;

    this.player = playerFlag;

    this.show(ctx);
  }

  show(ctx) {
    ctx.drawImage(this.sprite, this.x, this.y, this.width, this.height);
    this.person.show(ctx);
    if (this.hud) this.hud.show(ctx);
  }

  think(ctx, obstacles, yAxisMovement) {
    let nearestObstacles = this.find_nearest_obstacles(obstacles);
    let {gapLeft, gapRight, gapYPos} = findObstacleGap(nearestObstacles)
    var input = [
      this.x / ctx.canvas.width,
      this.endX / ctx.canvas.width,
      gapLeft / ctx.canvas.width,
      gapRight / ctx.canvas.width
    ];
    if(yAxisMovement) {
      input.push((this.y - gapYPos)/ctx.canvas.height);
    }
    let result = this.brain.predict(input);
    let left = result[0];
    let right = result[1];
    if(yAxisMovement) {
      var up = result[2];
      var down = result[3];
    }
    var keys = [];
    let highestResult = Math.max.apply(null, result);
    switch (highestResult) {
      case left:
        keys[37] = true;
        break;
      case right:
        keys[39] = true;
        break;
      case up:
        keys[38] = true;
        break;
      case down:
        keys[40] = true;
        break;
      default:
        break;
    }

    return keys;
  }

  update(ctx, keys, obstacles, newDistanceTraveled, yAxisMovement) {
    if (!this.player) keys = this.think(ctx, obstacles, yAxisMovement);
    let newX = this.moveToNewX(keys, ctx);
    let newY = this.moveToNewY(keys, ctx);
    super.update(newX, newY);
    this.person.update(ctx, this.x + this.width / 2, this.y + this.height);
    this.updateScore(ctx.canvas.height, newDistanceTraveled);
    this.show(ctx);
  }

  moveToNewX(keys, ctx) {
    let newX = this.x;
    if (keys && keys[37]) {
      if (this.x > 5) newX -= 5;
    }
    if (keys && keys[39]) {
      if (this.x + this.width + 5 < ctx.canvas.width) newX += 5;
    }
    return newX;
  }

  moveToNewY(keys, ctx) {
    let newY = this.y;
    if (keys && keys[38]) {
      if (this.y > 5) newY -= 5;
    }
    if (keys && keys[40]) {
      if (this.y + this.height + 5 < ctx.canvas.height) newY += 5;
    }
    return newY;
  }

  updateScore(canvas_height, newDistanceTraveled) {
    let heightOnScreen = 1 - this.y / canvas_height;
    this.score += Math.ceil(
      (newDistanceTraveled - this.distanceTraveled) * (heightOnScreen * 1.5)
    );
    this.distanceTraveled = newDistanceTraveled;
  }

  find_nearest_obstacles(obstacles) {
    if (obstacles.length <= 0) return [];
    let nearestObstacle1 = null;
    let nearestObstacle2 = null;
    let nearestDistance = 10000000;
    for (let i = 0; i < obstacles.length; i++) {
      let obstacle = obstacles[i];
      let distance = this.endY - obstacle.y;
      if (distance >= 0 && distance < nearestDistance) {
        nearestObstacle2 = nearestObstacle1 || obstacles[i + 1];
        nearestObstacle1 = obstacle;
        i++;
      }
    }
    if (nearestObstacle2) return [nearestObstacle1, nearestObstacle2];
    else if (nearestObstacle1) return [nearestObstacle1];
    else return [];
  }

  mutate(goalPercentage) {
    function mutateWeight(weight) {
      // if (Math.random(1) < 0.05) {
      //   console.log("mutate");
      //   return weight + randn_bm() * 0.5;
      // }
      // return weight;
      if (Math.random(1) < (0.10 * (1-goalPercentage))) {
        return weight + randn_bm() * 0.5;
      }
      return weight;
    }

    let input_weights = this.brain.input_weights.dataSync().map(mutateWeight);
    let input_shape = this.brain.input_weights.shape;
    this.brain.input_weights.dispose();
    this.brain.input_weights = tf.tensor(input_weights, input_shape);

    let output_weights = this.brain.output_weights.dataSync().map(mutateWeight);
    let output_shape = this.brain.output_weights.shape;
    this.brain.output_weights.dispose();
    this.brain.output_weights = tf.tensor(output_weights, output_shape);
  }

  updateSpeed() {
    if (this.keys && this.keys[38]) {
      this.boatSpeed += 2;
      if (this.boatSpeed > 200) {
        this.boatSpeed = 200;
      }
    } else if (this.keys && this.keys[40]) {
      this.boatSpeed -= 2;
      if (this.boatSpeed < 0) {
        this.boatSpeed = 0;
      }
    }
  }
}
