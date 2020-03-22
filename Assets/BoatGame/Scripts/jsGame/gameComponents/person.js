class Person extends GameComponent {
  constructor(ctx, ropeStartX, ropeStartY) {
    super();
    this.setComponentValues(
      ropeStartX,
      ropeStartY + 150,
      20,
      20,
      "images/person.png"
    );
    this.ropeStartX = ropeStartX;
    this.ropeStartY = ropeStartY;
    this.speedX = 0;
    this.speedY = 0;

    this.show(ctx);
  }

  update(ctx, ropeStartX, ropeStartY) {
    this.ropeStartX = ropeStartX;
    this.ropeStartY = ropeStartY;

    this.changePos(ropeStartX, ropeStartY);
    this.show(ctx);
  }

  show(ctx) {
    ctx.fillStyle = "green";
    ctx.drawImage(this.sprite, this.x, this.y, this.width, this.height);

    ctx.beginPath();
    ctx.moveTo(this.ropeStartX, this.ropeStartY);
    ctx.lineTo(this.x + this.width / 2, this.y);
    ctx.stroke();
  }

  changePos(ropeStartX, ropeStartY) {
    if (this.ropeLength(ropeStartX, ropeStartY) > 70)
      this.adjustVertical(ropeStartX);
    this.x += this.speedX;
    if (this.ropeLength(ropeStartX, ropeStartY) > 70)
      this.adjustHorizontal(ropeStartY);
    this.y += this.speedY;
  }

  adjustHorizontal(ropeStartY) {
    if (this.y < ropeStartY) {
      if (this.speedY != 0) {
        this.speedY *= -1;
        this.y += 5;
      } else this.speedY += 2;
    } else if (this.y > ropeStartY) {
      if (this.speedY != 0) {
        this.speedY *= -1;
        this.y -= 5;
      } else this.speedY -= 2;
    }
  }

  adjustVertical(ropeStartX) {
    if (this.x < ropeStartX) {
      if (this.speedX != 0) {
        this.speedX *= -1;
        this.x += 5;
      } else this.speedX += 2;
    } else if (this.x > ropeStartX) {
      if (this.speedX != 0) {
        this.speedX *= -1;
        this.x -= 5;
      } else this.speedX -= 2;
    }
  }

  ropeLength(ropeStartX, ropeStartY) {
    var a = Math.abs(ropeStartX - this.x);
    var b = Math.abs(ropeStartY - this.y);
    return Math.sqrt(a * a + b * b);
  }
}
