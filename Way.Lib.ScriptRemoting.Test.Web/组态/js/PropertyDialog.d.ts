declare function rgb(r: any, g: any, b: any): {
    "r": any;
    "g": any;
    "b": any;
};
declare class PropertyDialog {
    rootElement: HTMLElement;
    control: EditorControl;
    static CHKNumber: number;
    constructor(control: EditorControl);
    private setPointItemEvent(ele);
    private setChkItemEvent(ele);
    private setImgItemEvent(ele);
    private setRootEvent();
    private setInputEvent(input);
    hide(): void;
    show(): void;
}
