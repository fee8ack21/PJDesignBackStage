import { Component, HostListener } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  isWindowDisabled = false;

  constructor() {
    this.checkIfDisabled();
  }

  @HostListener('window:resize', ['$event'])
  onResize(e: any) {
    this.checkIfDisabled();
  }

  checkIfDisabled() {
    if (window.innerWidth < 1200) {
      this.isWindowDisabled = true;
      return;
    }

    this.isWindowDisabled = false;
  }
}
