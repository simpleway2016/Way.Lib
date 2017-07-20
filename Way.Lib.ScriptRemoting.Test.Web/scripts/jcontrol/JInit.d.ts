declare var AllJBinders: JBinder[];
declare class JElementHelper {
    static SystemTemplateContainer: HTMLElement;
    static replaceElement(source: HTMLElement, dst: HTMLElement): void;
    static getControlTypeName(tagname: string): string;
    static getElement(html: string): HTMLElement;
    static initElements(container: HTMLElement, bind: boolean): void;
}
