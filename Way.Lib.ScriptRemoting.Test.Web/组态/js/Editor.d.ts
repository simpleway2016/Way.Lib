declare class ToolBoxItem {
    constructor();
    begin(svgContainer: SVGSVGElement, position: any): void;
    mousemove(x: any, y: any): void;
    end(): EditorControl;
}
declare class ToolBox_Line extends ToolBoxItem {
    lineElement: SVGLineElement;
    constructor();
    begin(svgContainer: SVGSVGElement, position: any): void;
    mousemove(x: any, y: any): void;
    end(): EditorControl;
}
declare class ToolBox_Rect extends ToolBoxItem {
    rectElement: SVGRectElement;
    private startx;
    private starty;
    constructor();
    begin(svgContainer: SVGSVGElement, position: any): void;
    mousemove(x: any, y: any): void;
    end(): EditorControl;
}
declare class ToolBox_Ellipse extends ToolBoxItem {
    rootElement: SVGEllipseElement;
    private startx;
    private starty;
    constructor();
    begin(svgContainer: SVGSVGElement, position: any): void;
    mousemove(x: any, y: any): void;
    end(): EditorControl;
}
declare class ToolBox_Circle extends ToolBoxItem {
    rootElement: SVGCircleElement;
    private startx;
    private starty;
    constructor();
    begin(svgContainer: SVGSVGElement, position: any): void;
    mousemove(x: any, y: any): void;
    end(): EditorControl;
}
declare class Editor {
    private svgContainer;
    private currentToolBoxItem;
    private svgContainerMouseUpPosition;
    private beginedToolBoxItem;
    controls: any[];
    private selectingElement;
    constructor(id: string);
    selectControlsByRect(rect: any, ctrlKey: any): void;
    setCurrentToolBoxItem(typename: string): void;
    svgContainerClick(e: MouseEvent): void;
    svgContainerMouseMove(x: any, y: any): void;
}
