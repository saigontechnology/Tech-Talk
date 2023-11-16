import { ChangeDetectionStrategy, Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { isNil } from 'lodash-es';
import { MESSAGES } from './model/message.model';

@Component({
  selector: 'sct-common-message',
  templateUrl: './common-message.component.html',
  styleUrls: ['./common-message.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommonMessageComponent implements OnChanges {
  @Input() messageInfo: any;
  MESSAGES: any = MESSAGES;
  messageDisplay = '';

  constructor() {}

  ngOnChanges(changes: SimpleChanges): void {
    if(!isNil(this.messageInfo)){
      const messageKey = Object.keys(this.messageInfo)[0].toString().toLocaleUpperCase();
      this.messageDisplay = this.MESSAGES[messageKey];
    }
   
  }
}
