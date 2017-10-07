declare var AllSelectedControls: EditorControl[];
declare function documentElementMouseDown(e: MouseEvent): void;
declare class EditorControl {
    propertyDialog: PropertyDialog;
    ctrlKey: boolean;
    element: any;
    _selected: boolean;
    _moveAllSelectedControl: boolean;
    selected: boolean;
    rect: any;
    private mouseDownX;
    private mouseDownY;
    constructor(element: any);
    getPropertiesCaption(): string[];
    getProperties(): string[];
    getJson(): {
        tagName: any;
        rect: any;
        constructorName: any;
    };
    isIntersectWith(rect: any): boolean;
    isIntersect(rect1: any, rect: any): boolean;
    showProperty(): void;
    onSelectedChange(): void;
    onBeginMoving(): void;
    onMoving(downX: any, downY: any, nowX: any, nowY: any): void;
    onEndMoving(): void;
}
declare class SVGContainerControl extends EditorControl {
    rootElement: SVGSVGElement;
    colorBG: string;
    imgBg: string;
    bgWidth: string;
    bgHeight: string;
    getPropertiesCaption(): string[];
    getProperties(): string[];
    constructor(element: any);
    readonly rect: {
        x: number;
        y: number;
        width: number;
        height: number;
    };
}
declare class LineControl extends EditorControl {
    lineElement: SVGLineElement;
    pointEles: SVGCircleElement[];
    moving: boolean;
    startX: number;
    startY: number;
    valueX: any;
    valueY: any;
    rect: {
        x: number;
        y: number;
        width: number;
        height: number;
    };
    point: any;
    getJson(): any;
    lineWidth: string;
    color: string;
    constructor(element: any);
    getPropertiesCaption(): string[];
    getProperties(): string[];
    isIntersectWith(rect: any): boolean;
    onBeginMoving(): void;
    onMoving(downX: any, downY: any, nowX: any, nowY: any): void;
    onEndMoving(): void;
    onSelectedChange(): void;
    resetPointLocation(): void;
    setEvent(pointEle: any, xName: any, yName: any): void;
    pointMouseDown(e: MouseEvent, pointEle: any, xName: string, yName: string): void;
    pointMouseMove(e: MouseEvent, pointEle: any, xName: string, yName: string): void;
    pointMouseUp(e: MouseEvent, pointEle: any): void;
}
declare class RectControl extends EditorControl {
    rectElement: SVGGraphicsElement;
    pRightBottom: SVGCircleElement;
    moving: boolean;
    startX: number;
    startY: number;
    rect: any;
    strokeWidth: string;
    colorStroke: string;
    colorFill: string;
    getPropertiesCaption(): string[];
    getProperties(): string[];
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
    rect: any;
    strokeWidth: string;
    colorStroke: string;
    colorFill: string;
    getPropertiesCaption(): string[];
    getProperties(): string[];
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
    rect: any;
    strokeWidth: string;
    colorStroke: string;
    colorFill: string;
    getPropertiesCaption(): string[];
    getProperties(): string[];
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
declare class ImageControl extends RectControl {
    imgElement: SVGImageElement;
    imgDefault: string;
    getPropertiesCaption(): string[];
    getProperties(): string[];
    constructor(element: any);
}
declare class TextControl extends RectControl {
    textElement: SVGTextElement;
    selectingElement: SVGRectElement;
    text: string;
    size: number;
    colorFill: string;
    getPropertiesCaption(): string[];
    getProperties(): string[];
    rect: any;
    constructor(element: any);
    onSelectedChange(): void;
    resetPointLocation(): void;
}
