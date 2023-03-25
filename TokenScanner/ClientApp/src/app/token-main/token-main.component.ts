import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, Inject, NgZone, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Chart, ChartData, ChartOptions } from 'chart.js';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { Token } from '../../models/token'
import { TokenViewDialogComponent } from '../token-view-dialog/token-view-dialog.component';
import 'rxjs';

@Component({
  selector: 'app-token-main',
  templateUrl: './token-main.component.html',
  styleUrls: ['./token-main.component.css']
})

export class TokenMainComponent implements OnInit, AfterViewInit {
  private url!: string;

  constructor(private http: HttpClient,
    private fb: FormBuilder,
    public dialog: MatDialog,
    @Inject('BASE_URL') baseUrl: string,
    private zone: NgZone) {
    this.url = baseUrl + 'api/tokens';
  }

  chartData: ChartData<'doughnut'> = {
    labels: [],
    datasets: [
      {
        data: [],
      }
    ]
  };

  chartOptions: ChartOptions = {
    maintainAspectRatio: false,
    plugins: {
      title: {
        display: true,
        text: '',
      },
      legend: {
        display: false
      },
    }
  };

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild('tokenChart') tokenChart!: Chart;
  token!: Token;
  tokens: Token[] = [];
  tokenForm!: FormGroup;
  showLoader!: boolean;
  totalSupply!: number;
  names: string[] = [];
  suppliesByName: number[] = [];
  displayedColumns: string[] = ['tokenId', 'name', 'symbol', 'contractAddress', 'totalSupply', 'totalHolders', 'totalSupplyPercentage', 'action'];
  dataSource = new MatTableDataSource<Token>();

  ngOnInit(): void {
    this.tokenForm = this.fb.group({
      name: '',
      symbol: '',
      contractAddress: '',
      totalSupply: null,
      totalHolders: null,
      chartType: 'doughnut',
      token: null,
      showLegend: true
    });
    this.loadTokenData();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  loadTokenData() {
    this.showLoader = true;
    this.names = [];
    this.suppliesByName = [];
    console.log(this.url);
    this.http.get<Token[]>(this.url)
      .subscribe({
        next: (result) => {
          this.totalSupply = result.reduce((sum, current) => sum + current.totalSupply, 0);

          result.forEach(x => {
            let totalSupplyPercentage = (x.totalSupply / this.totalSupply) * 100;
            x.totalSupplyPercentage = totalSupplyPercentage;
            this.names.push(x.name);
            this.suppliesByName.push(x.totalSupply);
          });

          this.tokens = result;
          this.dataSource.data = this.tokens;
          this.dataSource.paginator = this.paginator;
          this.showTokenChart();
          this.resetTokenForm();
          this.showLoader = false;
        },
        error: (err) => {
          console.error(err);
          this.showLoader = false;
        },
        complete: () => console.info('Get tokens completed')
      });
  }

  showTokenChart() {
    this.chartData = {
      labels: this.names,
      datasets: [
        {
          data: this.suppliesByName,
        }
      ]
    };

    this.chartOptions = {
      maintainAspectRatio: false,
      plugins: {
        title: {
          display: true,
          text: 'Total Statistics by Total Supply',
        },
        legend: {
          display: false
        },
      },
      onClick: (event: any, elements: any, chart: any) => {
        if (elements[0]) {
          let i = elements[0].index;
          this.handleChartClick(chart.data.labels[i]);
        }
      }
    };
    this.showLoader = false;
  }

  handleChartClick(name: string) {
    let token = this.tokens.find((element) => {
      return element.name == name;
    });

    this.showTokenDetails(token as Token);
  }

  saveOrUpdateToken() {
    this.showLoader = true;

    this.token = this.tokenForm.value;

    this.http.post<Token>(this.url, this.token)
      .subscribe({
        next: () => {
          this.loadTokenData();
          this.showLoader = false;
        },
        error: (err) => {
          console.error(err);
          this.tokenForm.patchValue({
            token: ''
          });
          this.showLoader = false;
        },
        complete: () => console.info('Populate token completed')
      });
  }

  resetTokenForm() {
    this.tokenForm.reset({
      token: this.tokens,
      chartType: 'doughnut',
      showLegend: true
    })
  }

  showTokenDetails(item: Token): void {
    this.zone.run(() => {
      this.dialog.open(TokenViewDialogComponent, {
        data: item
      });
    })
  }

  populateTokenForm(item: Token) {
    this.tokenForm.patchValue(
      {
        name: item.name,
        symbol: item.symbol,
        contractAddress: item.contractAddress,
        totalSupply: item.totalSupply,
        totalHolders: item.totalHolders,
      });
  }

  export() {
    this.http.get(this.url + '/export', {
      headers: {
        'Content-Type': 'application/json'
      },
      responseType: 'blob'
    })
      .subscribe(responseData => {
        let dataType = responseData.type.toString();
        let binaryData: BlobPart[] = [];
        binaryData.push(responseData);
        let downloadLink = document.createElement('a');
        downloadLink.href = window.URL.createObjectURL(new Blob(binaryData, { type: dataType }));
        downloadLink.download = "tokens.csv";
        document.body.appendChild(downloadLink);
        downloadLink.click();
      });
  }

  deleteToken(id: number, symbol: string) {
    if (confirm(`Are you sure you want to delete token ${symbol}`)) {
      this.http.delete(this.url + '/' + id)
        .subscribe({
          next: () => {
            this.loadTokenData();
            this.showLoader = false;
          },
          error: (err) => {
            console.error(err);
            this.tokenForm.patchValue({
              token: ''
            });
          },
          complete: () => console.info('Populate token completed')
        });
    }
  }
}
