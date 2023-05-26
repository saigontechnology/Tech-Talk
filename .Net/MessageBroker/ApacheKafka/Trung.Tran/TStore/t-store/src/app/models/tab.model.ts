export class TabModel {
    contents: { content: string, time: Date }[];

    constructor(public title: string) {
        this.contents = [];
    }
}