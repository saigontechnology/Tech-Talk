import { Component, HostListener, OnInit } from '@angular/core';

import { NzMessageService } from 'ng-zorro-antd/message';

import { AppStateService } from './services/app-state.service';
import { InteractionService } from './services/interaction.service';
import { NotificationService } from './services/notification.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 't-store';

  constructor(private _appStateServuce: AppStateService,
    private _messageService: NzMessageService,
    private _notificationService: NotificationService,
    private _interactionService: InteractionService) {
  }

  ngOnInit(): void {
    this._inputUsername();
    this._notificationService.listenNotifications();

    // [DEMO] concurrent users, stress testing
    fetch('/assets/dynamic-config.json').then(resp => resp.json()).then((config: {
      stressTestEnabled: boolean
    }) => {
      if (config.stressTestEnabled) {
        setInterval(() => {
          this._interactionService.saveClick(1).subscribe({});
        }, 100);
      }
    })
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    this._interactionService.saveClick(1).subscribe({
      next: () => {
        console.log('Save click');
      },
      error: (err) => {
        console.log('Error: ', err);
      }
    });
  }

  @HostListener('document:dblclick', ['$event'])
  onDocumentDblClick(event: MouseEvent) {
    this._interactionService.saveClick(2).subscribe({
      next: () => {
        console.log('Save dblclick');
      },
      error: (err) => {
        console.log('Error: ', err);
      }
    });
  }

  onComponentActivate(event: any) {
    const page = location.href;
    this._appStateServuce.setLastPage(page);
    this._interactionService.saveAccess(page).subscribe({
      next: () => {
        console.log('Save access page: ', page);
      },
      error: (err) => {
        console.log('Error: ', err);
      }
    });;
  }

  private _inputUsername() {
    let userName: string | null = sessionStorage.getItem('ts-username');
    if (!userName?.length) {
      do {
        userName = prompt('Input your username:') || '';
        userName = userName.trim();
      }
      while (!userName?.length);
      sessionStorage.setItem('ts-username', userName);
    }
    this._appStateServuce.setUserName(userName);
  }
}
