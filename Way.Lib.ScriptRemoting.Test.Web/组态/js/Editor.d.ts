declare var fileBrowser: FileBrowser;
declare var ServerUrl: string;
declare var windowGuid: number;
declare var CtrlKey: boolean;
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
declare class ToolBox_HistoryTrend extends ToolBoxItem {
    control: HistoryTrendControl;
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
    writeValue(pointName: any, addr: any, value: any): any;
    isIdExist(id: string): boolean;
    getControl(id: string): EditorControl;
}
declare class Editor implements IEditorControlContainer {
    getControl(id: string): EditorControl;
    removeControl(ctrl: EditorControl): void;
    addControl(ctrl: EditorControl): void;
    writeValue(pointName: any, addr: any, value: any): void;
    name: string;
    code: string;
    divContainer: HTMLElement;
    svgContainer: SVGSVGElement;
    private currentToolBoxItem;
    private svgContainerMouseUpPosition;
    private beginedToolBoxItem;
    propertyDialog: PropertyDialog;
    controls: any[];
    private selectingElement;
    undoMgr: UndoManager;
    changed: boolean;
    windowWidth: any;
    windowHeight: any;
    editingPointTextbox: any;
    colorBG: string;
    imgBg: string;
    bgWidth: string;
    bgHeight: string;
    private _customProperties;
    customProperties: string;
    getPropertiesCaption(): string[];
    getProperties(): string[];
    scrollTop: number;
    scrollLeft: number;
    constructor(id: string);
    private initMoveToScrollEvent();
    private initScaleEvent();
    private initDivContainer();
    isWatchingRect: boolean;
    isRunMode: boolean;
    run(): void;
    createGroupControl(windowCode: any, rect: any): GroupControl;
    rebuildControls(): void;
    getScript(): string;
    currentScale: number;
    scale(_scale: any): void;
    undo(): void;
    redo(): void;
    selectAll(): void;
    selectWebControlByPointName(pointName: string): void;
    group(): void;
    ungroup(): void;
    delete(): void;
    resetScrollbar(): void;
    save(): void;
    getSaveInfo(): string;
    cut(): void;
    copy(): void;
    paste(): void;
    isIdExist(id: string): boolean;
    fireBodyEvent(event: any): void;
    selectControlsByRect(rect: any): void;
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
