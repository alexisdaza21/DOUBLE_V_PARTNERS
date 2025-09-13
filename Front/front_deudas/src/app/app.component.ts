import { Component } from '@angular/core';
import { CommonService } from './services/common/common.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'front_deudas';
  
  constructor(private commonService: CommonService) {

    this.commonService.getTokenAnonimo()
    .subscribe( (response: any) => {
      debugger
      if (response.status === 200) {
        localStorage.setItem('hsJwt', response.data);
      }
    }
  );
  }




}
