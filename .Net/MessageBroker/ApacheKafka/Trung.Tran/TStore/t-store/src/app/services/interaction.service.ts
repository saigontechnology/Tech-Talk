import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { ActionType } from '../constants/interaction.const';
import { dynamicUrls } from '../constants/urls.const';

import { parseUrl } from '../helpers/url.helper';

import { InteractionModel } from '../models/interaction.model';
import { InteractionReportModel } from '../models/interaction-report.model';

import { AppStateService } from './app-state.service';


@Injectable({
  providedIn: 'root'
})
export class InteractionService {

  constructor(private _httpClient: HttpClient,
    private _appStateService: AppStateService) { }

  saveClick(count: number) {
    const url = parseUrl('/api/interactions', dynamicUrls.interactionApiUrl);
    const model: InteractionModel = {
      action: ActionType.Click,
      userName: this._appStateService.userName,
      fromPage: location.href,
      clickCount: count
    };
    return this._httpClient.post(url.toString(), model);
  }

  saveSearch(searchValue: string) {
    const url = parseUrl('/api/interactions', dynamicUrls.interactionApiUrl);
    const model: InteractionModel = {
      action: ActionType.Search,
      userName: this._appStateService.userName,
      fromPage: location.href,
      searchTerm: searchValue
    };
    return this._httpClient.post(url.toString(), model);
  }

  saveAccess(page: string) {
    const url = parseUrl('/api/interactions', dynamicUrls.interactionApiUrl);
    const model: InteractionModel = {
      action: ActionType.AccessPage,
      userName: this._appStateService.userName,
      fromPage: this._appStateService.lastPage,
      toPage: page
    };
    return this._httpClient.post(url.toString(), model);
  }

  getInteractionReports() {
    const url = parseUrl('/api/interactions/reports', dynamicUrls.interactionApiUrl);
    return this._httpClient.get<InteractionReportModel[]>(url.toString());
  }
}
