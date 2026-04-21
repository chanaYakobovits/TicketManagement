import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HandleTicket } from './handle-ticket';

describe('HandleTicket', () => {
  let component: HandleTicket;
  let fixture: ComponentFixture<HandleTicket>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HandleTicket]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HandleTicket);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
