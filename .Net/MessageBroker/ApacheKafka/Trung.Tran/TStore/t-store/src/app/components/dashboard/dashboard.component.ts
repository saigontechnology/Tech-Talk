import { Component, OnDestroy, OnInit } from '@angular/core';

import { Subject, takeUntil } from 'rxjs';
import { NzMessageService } from 'ng-zorro-antd/message';

import { ActionType } from 'src/app/constants/interaction.const';
import { tabTitles } from 'src/app/constants/dashboard.const';

import { TabModel } from 'src/app/models/tab.model';
import { InteractionModel } from 'src/app/models/interaction.model';

import { AppStateService } from 'src/app/services/app-state.service';
import { InteractionService } from 'src/app/services/interaction.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit, OnDestroy {

  loading: boolean;
  clickCount: number;
  searchCount: number;
  accessCount: number;
  tabs: TabModel[];

  private _destroy$: Subject<any>;

  constructor(
    private _interactionService: InteractionService,
    private _messageService: NzMessageService,
    private _appStateService: AppStateService) {
    this.clickCount = 0;
    this.searchCount = 0;
    this.accessCount = 0;
    this.loading = false;
    this._destroy$ = new Subject();
    this.tabs = Object.values(tabTitles).map(title => new TabModel(title));
  }

  ngOnInit(): void {
    this._fetchReports();
    this._handleInteractionReportUpdated();
    this._handleUnusualSearchs();
    this._handleNewInteractions();
    this._handleNewLogData();
  }

  ngOnDestroy(): void {
    this._destroy$.next(true);
  }

  private _handleNewInteractions() {
    this._appStateService.newInteractions$
      .pipe(takeUntil(this._destroy$))
      .subscribe(interactions => {
        const time = new Date();
        const getContent = (interaction: InteractionModel) => {
          switch (interaction.action) {
            case ActionType.AccessPage: return `User ${interaction.userName} accessed ${interaction.toPage} at ${interaction.time}`;
            case ActionType.Search: return `User ${interaction.userName} searched ${interaction.searchTerm} at ${interaction.time}`;
            case ActionType.Click: return `User ${interaction.userName} clicked ${interaction.clickCount} time(s) on page ${interaction.fromPage} at ${interaction.time}`;
          }
        };
        const contents = interactions.map(interaction => ({
          content: getContent(interaction),
          time
        }));
        const tab = this.tabs.find(tab => tab.title === tabTitles.interactions);
        if (tab) {
          tab.contents = [...contents, ...tab.contents];
        }
      });
  }

  private _handleUnusualSearchs() {
    this._appStateService.unusualSearchs$
      .pipe(takeUntil(this._destroy$))
      .subscribe(unusualSearchs => {
        const time = new Date();
        const contents = unusualSearchs.map(s => ({
          content: `User ${s.userName} performed an unusual search '${s.searchTerm}' at ${s.time}`,
          time
        }));
        const tab = this.tabs.find(tab => tab.title === tabTitles.unusualSearchs);
        if (tab) {
          tab.contents = [...contents, ...tab.contents];
        }
      });
  }

  private _handleInteractionReportUpdated() {
    this._appStateService.interactionReportUpdated$
      .pipe(takeUntil(this._destroy$))
      .subscribe(report => {
        switch (report.action) {
          case ActionType.AccessPage: this.accessCount = report.count; break;
          case ActionType.Search: this.searchCount = report.count; break;
          case ActionType.Click: this.clickCount = report.count; break;
        }
      });
  }

  private _handleNewLogData() {
    this._appStateService.logData$
      .pipe(takeUntil(this._destroy$))
      .subscribe(logData => {
        const time = new Date();
        const content = {
          content: logData.logLine,
          time
        };
        let tab = this.tabs.find(tab => tab.title === logData.id);

        if (!tab) {
          tab = new TabModel(logData.id);
          this.tabs = [...this.tabs, tab];
        }

        if (tab) {
          tab.contents = [content, ...tab.contents];
        }
      });
  }

  private _fetchReports() {
    this.loading = true;
    const finishFetching = () => this.loading = false;
    this._interactionService.getInteractionReports().subscribe({
      next: reports => {
        this.accessCount = reports.find(r => r.action === ActionType.AccessPage)?.count || 0;
        this.searchCount = reports.find(r => r.action === ActionType.Search)?.count || 0;
        this.clickCount = reports.find(r => r.action === ActionType.Click)?.count || 0;
      },
      error: () => {
        this._messageService.error('Error fetching reports');
        finishFetching();
      },
      complete: () => finishFetching()
    });
  }

}
