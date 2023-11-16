import { Component, OnInit, Input,ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,

})
export class LoaderComponent implements OnInit {
  @Input() isShow = false;
  constructor() {}

  ngOnInit(): void {}
}
