declare var fileBrowser: FileBrowser;
declare var windowid: number;
declare class ToolBoxItem {
    buildDone: (control: EditorControl) => any;
    readonly supportMove: boolean;
    constructor();
    begin(svgContainer: SVGSVGElement, position: any): void;
    mousemove(x: any, y: any): void;
    end(): EditorControl;
}
declare class ToolBox_Line extends ToolBoxItem {
    control: LineControl;
    constructor();
    begin(svgContainer: SVGSVGElement, position: any): void;
    mousemove(x: any, y: any): void;
    end(): EditorControl;
}
declare class ToolBox_Rect extends ToolBoxItem {
    control: RectControl;
    private startx;
    private starty;
    constructor();
    begin(svgContainer: SVGSVGElement, position: any): void;
    mousemove(x: any, y: any): void;
    end(): EditorControl;
}
declare class ToolBox_Ellipse extends ToolBoxItem {
    control: EllipseControl;
    private startx;
    private starty;
    constructor();
    begin(svgContainer: SVGSVGElement, position: any): void;
    mousemove(x: any, y: any): void;
    end(): EditorControl;
}
declare class ToolBox_Circle extends ToolBoxItem {
    control: CircleControl;
    private startx;
    private starty;
    constructor();
    begin(svgContainer: SVGSVGElement, position: any): void;
    mousemove(x: any, y: any): void;
    end(): EditorControl;
}
declare class ToolBox_Image extends ToolBoxItem {
    control: ImageControl;
    readonly supportMove: boolean;
    constructor();
    begin(svgContainer: SVGSVGElement, position: any): void;
}
declare class ToolBox_Text extends ToolBoxItem {
    control: TextControl;
    readonly supportMove: boolean;
    constructor();
    begin(svgContainer: SVGSVGElement, position: any): void;
}
declare class ToolBox_Cylinder extends ToolBoxItem {
    control: CylinderControl;
    private startx;
    private starty;
    constructor();
    begin(svgContainer: SVGSVGElement, position: any): void;
    mousemove(x: any, y: any): void;
    end(): EditorControl;
}
declare class ToolBox_Trend extends ToolBoxItem {
    control: TrendControl;
    private startx;
    private starty;
    constructor();
    begin(svgContainer: SVGSVGElement, position: any): void;
    mousemove(x: any, y: any): void;
    end(): EditorControl;
}
declare class ToolBox_ButtonArea extends ToolBoxItem {
    control: ButtonAreaControl;
    private startx;
    private starty;
    constructor();
    begin(svgContainer: SVGSVGElement, position: any): void;
    mousemove(x: any, y: any): void;
    end(): EditorControl;
}
interface IEditorControlContainer {
    controls: any[];
    addControl(ctrl: EditorControl): any;
    removeControl(ctrl: EditorControl): any;
}
declare class Editor implements IEditorControlContainer {
    removeControl(ctrl: EditorControl): void;
    addControl(ctrl: EditorControl): void;
    private svgContainer;
    private currentToolBoxItem;
    private svgContainerMouseUpPosition;
    private beginedToolBoxItem;
    propertyDialog: PropertyDialog;
    controls: any[];
    private selectingElement;
    undoMgr: UndoManager;
    colorBG: string;
    imgBg: string;
    bgWidth: string;
    bgHeight: string;
    getPropertiesCaption(): string[];
    getProperties(): string[];
    constructor(id: string);
    undo(): void;
    redo(): void;
    delete(): void;
    copy(): void;
    paste(): void;
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
