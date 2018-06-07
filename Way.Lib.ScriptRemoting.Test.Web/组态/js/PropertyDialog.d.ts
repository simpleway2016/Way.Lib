declare function rgb(r: any, g: any, b: any): {
    "r": any;
    "g": any;
    "b": any;
};
declare class PropertyDialog {
    rootElement: HTMLElement;
    control: EditorControl;
    documentClickEvent: any;
    isShowed: boolean;
    static CHKNumber: number;
    constructor(control: EditorControl);
    private _documentElementClick(e);
    dispose(): void;
    private setPointItemEvent(ele);
    private setChkItemEvent(ele);
    private setImgItemEvent(ele);
    private setRootEvent();
    private setInputEvent(input);
    hide(): void;
    show(): void;
}
