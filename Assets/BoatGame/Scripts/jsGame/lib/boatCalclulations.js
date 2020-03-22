function findObstacleGap(nearestObstacles) {
  if (nearestObstacles.length > 1) {
    if (nearestObstacles[0].x < nearestObstacles[1].x) {
      var nearestObstacle1 = nearestObstacles[0];
      var nearestObstacle2 = nearestObstacles[1];
    } else {
      var nearestObstacle2 = nearestObstacles[0];
      var nearestObstacle1 = nearestObstacles[1];
    }
    var gapLeft = nearestObstacle1.endX;
    var gapRight = nearestObstacle2.x;
    var gapYPos = nearestObstacle1.endY;
  } else if (nearestObstacles.length > 0) {
    let nearestObstacle1 = nearestObstacles[0];
    var gapLeft = nearestObstacle1.endX;
    var gapRight = 0;
    var gapYPos = nearestObstacle1.endY;
  } else {
    var gapLeft = 0;
    var gapRight = 0;
    var gapYPos = 0;
  }

  return {
    gapLeft,
    gapRight,
    gapYPos
  }
}