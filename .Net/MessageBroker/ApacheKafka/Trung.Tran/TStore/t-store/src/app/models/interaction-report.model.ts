import { ActionType } from "../constants/interaction.const";

export interface InteractionReportModel {
    id: string;
    action: ActionType;
    count: number;
}