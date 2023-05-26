import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { TokenInterceptorService } from './token-interceptor.service';

/** Http interceptor providers in outside-in order */
export const HTTP_INTERCEPTORS_PROVIDERS = [
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptorService, multi: true },
];