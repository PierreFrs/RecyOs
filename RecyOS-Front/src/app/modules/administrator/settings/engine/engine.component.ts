import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-engine',
    templateUrl: './engine.component.html',
    styleUrls: ['./engine.component.scss']
})
export class EngineComponent implements OnInit {
    engineForm: FormGroup;

    constructor(private readonly _formBuilder: FormBuilder) {
        this.engineForm = this._formBuilder.group({
            myParameter: [null, Validators.required],
            ShipperToken: [null, Validators.required],
            ShipperEndPoint: [null, Validators.required],
            IsActive: [null, Validators.required]
        });
    }

    ngOnInit(): void {
        this.engineForm.valueChanges.subscribe(value => {
            
        });
    }

    onSubmit(): void {
        if (this.engineForm.valid) {
        //
        }
    }
} 