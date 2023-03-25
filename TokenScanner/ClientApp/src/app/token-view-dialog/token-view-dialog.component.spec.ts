import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TokenViewDialogComponent } from './token-view-dialog.component';

describe('TokenViewDialogComponent', () => {
  let component: TokenViewDialogComponent;
  let fixture: ComponentFixture<TokenViewDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TokenViewDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TokenViewDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
