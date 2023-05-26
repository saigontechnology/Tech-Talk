import { NotificationType } from "../constants/notification.const";

export interface NotificationModel {
    type: NotificationType;
    data: any;
}
