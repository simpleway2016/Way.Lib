declare var fileBrowser: FileBrowser;
declare class ToolBoxItem {
    buildDone: (control: EditorControl) => any;
    readonly supportMove: boolean;
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
declare class ToolBox_Image extends ToolBoxItem {
    rootElement: SVGImageElement;
    readonly supportMove: boolean;
    constructor();
    begin(svgContainer: SVGSVGElement, position: any): void;
}
declare class ToolBox_Text extends ToolBoxItem {
    rootElement: SVGTextElement;
    readonly supportMove: boolean;
    constructor();
    begin(svgContainer: SVGSVGElement, position: any): void;
}
declare class Editor {
    private svgContainer;
    private currentToolBoxItem;
    private svgContainerMouseUpPosition;
    private beginedToolBoxItem;
    propertyDialog: PropertyDialog;
    controls: any[];
    private selectingElement;
    constructor(id: string);
    fireBodyEvent(event: any): void;
    selectControlsByRect(rect: any, ctrlKey: any): void;
    setCurrentToolBoxItem(typename: string): void;
    svgContainerClick(e: MouseEvent): void;
    svgContainerMouseMove(x: any, y: any): void;
    setting(e: MouseEvent): void;
    private svgContainerClickForDialog(e);
    alignLeft(): void;
    alignRight(): void;
    alignTop(): void;
    alignBottom(): void;
    hSpacing(): void;
    vSpacing(): void;
    hCenter(): void;
    vCenter(): void;
    layerUp(): void;
    layerDown(): void;
    layerFront(): void;
    layerBottom(): void;
    getIndex(element: any): number;
}
