declare var AllSelectedControls: EditorControl[];
declare function documentElementMouseDown(e: MouseEvent): void;
declare class EditorControl {
    ctrlKey: boolean;
    element: any;
    _selected: boolean;
    _moveAllSelectedControl: boolean;
    selected: boolean;
    private mouseDownX;
    private mouseDownY;
    constructor(element: any);
    isIntersectWith(rect: any): boolean;
    isIntersect(rect1: any, rect: any): boolean;
    onSelectedChange(): void;
    onBeginMoving(): void;
    onMoving(downX: any, downY: any, nowX: any, nowY: any): void;
    onEndMoving(): void;
}
declare class LineControl extends EditorControl {
    lineElement: SVGLineElement;
    pointEles: SVGCircleElement[];
    moving: boolean;
    startX: number;
    startY: number;
    valueX: any;
    valueY: any;
    constructor(element: any);
    isIntersectWith(rect: any): boolean;
    onBeginMoving(): void;
    onMoving(downX: any, downY: any, nowX: any, nowY: any): void;
    onEndMoving(): void;
    onSelectedChange(): void;
    setEvent(pointEle: any, xName: any, yName: any): void;
    pointMouseDown(e: MouseEvent, pointEle: any, xName: string, yName: string): void;
    pointMouseMove(e: MouseEvent, pointEle: any, xName: string, yName: string): void;
    pointMouseUp(e: MouseEvent, pointEle: any): void;
}
declare class RectControl extends EditorControl {
    rectElement: SVGRectElement;
    pRightBottom: SVGCircleElement;
    moving: boolean;
    startX: number;
    startY: number;
    constructor(element: any);
    isIntersectWith(rect: any): boolean;
    onSelectedChange(): void;
    resetPointLocation(): void;
    setEvent(pointEle: any): void;
    pointMouseDown(e: MouseEvent, pointEle: any): void;
    pointMouseMove(e: MouseEvent, pointEle: any): void;
    pointMouseUp(e: MouseEvent, pointEle: any): void;
    onBeginMoving(): void;
    onMoving(downX: any, downY: any, nowX: any, nowY: any): void;
    onEndMoving(): void;
}
declare class EllipseControl extends EditorControl {
    rootElement: SVGEllipseElement;
    pointEles: SVGCircleElement[];
    moving: boolean;
    startX: number;
    startY: number;
    constructor(element: any);
    isIntersectWith(rect: any): boolean;
    onSelectedChange(): void;
    resetPointLocation(): void;
    setEvent(pointEle: any): void;
    pointMouseDown(e: MouseEvent, pointEle: any): void;
    pointMouseMove(e: MouseEvent, pointEle: any): void;
    pointMouseUp(e: MouseEvent, pointEle: any): void;
    onBeginMoving(): void;
    onMoving(downX: any, downY: any, nowX: any, nowY: any): void;
    onEndMoving(): void;
}
declare class CircleControl extends EditorControl {
    rootElement: SVGCircleElement;
    pointEles: SVGCircleElement[];
    moving: boolean;
    startX: number;
    startY: number;
    constructor(element: any);
    isIntersectWith(rect: any): boolean;
    onSelectedChange(): void;
    resetPointLocation(): void;
    setEvent(pointEle: any): void;
    pointMouseDown(e: MouseEvent, pointEle: any): void;
    pointMouseMove(e: MouseEvent, pointEle: any): void;
    pointMouseUp(e: MouseEvent, pointEle: any): void;
    onBeginMoving(): void;
    onMoving(downX: any, downY: any, nowX: any, nowY: any): void;
    onEndMoving(): void;
}
