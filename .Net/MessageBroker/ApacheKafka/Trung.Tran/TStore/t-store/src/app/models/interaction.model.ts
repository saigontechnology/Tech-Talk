import { ActionType } from "../constants/interaction.const";

export interface InteractionModel {
    id?: string;
    action: ActionType;
    fromPage: string;
    toPage?: string;
    searchTerm?: string;
    clickCount?: number;
    userName: string;
    time?: string;
}