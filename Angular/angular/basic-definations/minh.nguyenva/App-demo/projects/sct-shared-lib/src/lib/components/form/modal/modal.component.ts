import { MODAL_STATUS } from './modal.model';
import { ChangeDetectionStrategy, Component,Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'sct-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ModalComponent  {

  @Input() modalHeader = '';
  @Input() modalMessage = '';
  @Input() yesText = 'Yes';
  @Input() cancelText = 'Cancel';
  @Input() showLoading = false;
  @Input() showFullButtons = true;

  MODAL_STATUS = MODAL_STATUS;

  constructor(public activeModal: NgbActiveModal) { }

  close(param: string) {
    this.activeModal.close(param);
  }
}
