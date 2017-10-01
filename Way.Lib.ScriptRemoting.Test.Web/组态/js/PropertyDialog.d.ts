declare class PropertyDialog {
    rootElement: HTMLElement;
    control: EditorControl;
    constructor(control: EditorControl);
    private setRootEvent();
    private setInputEvent(input);
    hide(): void;
    show(): void;
}
