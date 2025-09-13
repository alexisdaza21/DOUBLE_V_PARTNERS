import { Component } from '@angular/core';
import { CommonService } from './services/common/common.service';
import { environment } from '../Eviroments/enviroments';
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
        environment.hsJwt = response.data;
      }
    }
  );
  }




}
