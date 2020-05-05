import { AnnotationSource } from './annotation-source.model';

export class AnnotationTarget {
    public id = 0;
    public coordinateX: number;
    public coordinateY: number;
    public width: number;
    public height: number;
    public sourceId: number;
    public source: AnnotationSource;

    get right(): number { return this.coordinateY + this.width; }
    get bottom(): number { return this.coordinateX + this.height; }

    constructor(coordinateX?: number, coordinateY?: number, width?: number, height?: number) {
        this.coordinateX = coordinateX || 0;
        this.coordinateY = coordinateY || 0;
        this.width = width || 0;
        this.height = height || 0;
    }

    public contains(x: number, y: number): boolean {
        return this.coordinateX <= x && x <= this.coordinateX + this.width &&
            this.coordinateY <= y && y <= this.coordinateY + this.height;
    }

    public intersectRect(target: AnnotationTarget) {
        return !(target.coordinateY > this.right ||
                 target.right < this.coordinateY ||
                 target.coordinateX > this.bottom ||
                 target.bottom < this.coordinateX);
      }

    public draw(ctx: CanvasRenderingContext2D): void {
        if (this.coordinateX + this.width > 0 && this.coordinateY + this.height > 0) {
            const rect = new Path2D();
            rect.rect(this.coordinateX, this.coordinateY, this.width, this.height);
            const rectangle = new Path2D(rect);
            ctx.stroke(rectangle);
        }
    }

    public highlight(ctx: CanvasRenderingContext2D): void {
        ctx.fillRect(this.coordinateX, this.coordinateY, this.width, this.height);
    }

    public clearHighlight(ctx: CanvasRenderingContext2D): void {
        ctx.clearRect(this.coordinateX, this.coordinateY, this.width, this.height);
    }
}
