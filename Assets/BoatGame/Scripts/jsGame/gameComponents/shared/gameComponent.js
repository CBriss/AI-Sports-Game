class GameComponent {
  constructor() {}

  setComponentValues(x, y, width, height, imageUrl, canvas_width) {
    this.width = width ? width : this.randomWidth(canvas_width);
    this.height = height;
    this.x = x >= 0 ? x : this.randomXValue(canvas_width);
    this.y = y;
    this.endX = this.x + this.width;
    this.endY = this.y + this.height;

    this.sprite = new Image();
    this.sprite.src = imageUrl;
  }

  update(x, y) {
    this.x = x;
    this.y = y;
    this.endY = this.y + this.height;
    this.endX = this.x + this.width;
  }

  randomXValue(canvas_width) {
    var newX = 0;
    if (Math.random() > 0.5) {
      newX = canvas_width - this.width;
    }
    return newX;
  }

  randomWidth(canvas_width) {
    return (Math.random() * canvas_width + 100) / 2;
  }

  hasCollsion(otherobj) {
    var left = otherobj.x;
    var right = otherobj.endX;
    var top = otherobj.y;
    var bottom = otherobj.endY;

    if (
      (this.y >= top && this.y <= bottom) ||
      (this.endY >= top && this.endY <= bottom) ||
      (this.y <= top && this.endY >= bottom)
    ) {
      if (
        (this.x >= left && this.x <= right) ||
        (this.endX >= left && this.endX <= right)
      ) {
        return true;
      }
    }
    return false;
  }

  // Slight Optimization on collision detection
  // Stops looking for collisions with objects far away on vertical plane
  hasCollsionStortedArray(sortedArray, canvasMidPoint) {
    let verticalState = this.y < canvasMidPoint ? "Top" : "Bottom";
    if (verticalState == "Top") {
      var i = 0;
      while (
        i < sortedArray.length &&
        sortedArray[i].y <= canvasMidPoint &&
        sortedArray[i].y <= this.endY
      ) {
        if (this.hasCollsion(sortedArray[i])) return sortedArray[i];
        i++;
      }
    } else if (verticalState == "Bottom") {
      var i = sortedArray.length - 1;
      while (
        i >= 0 &&
        sortedArray[i].endY >= canvasMidPoint &&
        sortedArray[i].y <= this.endY
      ) {
        if (this.hasCollsion(sortedArray[i])) return sortedArray[i];
        i--;
      }
    }
    return false;
  }
}
