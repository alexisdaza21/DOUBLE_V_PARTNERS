import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListarAbonosComponent } from './listar-abonos.component';

describe('ListarAbonosComponent', () => {
  let component: ListarAbonosComponent;
  let fixture: ComponentFixture<ListarAbonosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ListarAbonosComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListarAbonosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
