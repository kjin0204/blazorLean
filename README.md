1. section1:8.Bonus_ 용돈 나누기앱 만들기
    1) <input type="range" max="@(item.Amount + Remaining)" step="1000" @bind="@item.Amount" @bind:event="oninput" style="width: @(100 * (item.Amount + Remaining) / totalBudget)%" />
        1 step="1000" 한stemp에 1000씩 변경됨.
        2 @bind:event="oninput" 데이터를 입력 할때마다 바인딩 됨.
        3 style="width: @(100 * (item.Amount + Remaining) / totalBudget)%" : UI에 비율에 맞게 전체 범위가 조정됨
    2) ToString("c0") : 원화로 표시 해줌
    3) (HTML속성)disabled : 컨트롤러를 비활성화 시키는 속성
    4) @inject IJSRuntime JS : razor에서 자바스크립트 사용 가능하게 함

2. section1:9.BlazorForm_EditForm 컴포넌트를 사용하여 폼 작성 및 유효성 검사 진행하기
    1) 모델에서 멤버에 어노테이션 추가 : using System.ComponentModel.DataAnnotations;을 추가 해 줘야 어노테이션 사용 가능
        [Required(ErrorMessage = "이름은 필수입니다.")] : 반드시 입력이 필요하게 함.
        [StringLength(10, ErrorMessage = "이름을 10자 이하로 입력하세요.")] : 글자수 제한을 줌
    2) 테그 <EditForm Model="@exampleModel" OnValidSubmit="@btnSubmit_Click"> 블레이저에서 제공하는 컴포넌트 Form과 비슷한 역활을 함.
        - Model : 어떤 모델을 사용할 지 지정함.
        - OnValidSubmit : 서브밋 할때 실행할 메서드 연결.
    3) <DataAnnotationsValidator></DataAnnotationsValidator>
        - 어노테이션 유효성 검사
    
    4) <ValidationSummary></ValidationSummary>
        - 에러메시지를 summary해서 보여줌.

    5) <ValidationMessage For="@(() => exampleModel.Name)"></ValidationMessage>
        - 특정 어노테이션의 에러 메시지를 선택해서 보여줌.

3. section1:10 ParentChild_부모 컴포넌트에서 자식 컴포넌트로 또는 그 반대로 데이터 주고 받기
     1) 부모에서 자식으로 값 전달 :[Parameter] 어노테이션을 변수에 설정 하면 부모 에서 자식 변수에 값을 넘겨줄 수 있음.
        예) 자식 컴포넌트 name = FrmChild, 변수 int test
            <FrmChild test="1234"> </FrmChild>
    2) 자식에서 부모로 값 전달. Callback 함수를 자식으로 넘겨줘서 자식 폼에서 실행
    3) 자바 스크립트 alert 사용 방법
        - @inject IJSRuntime js 추가.
        - js.InvokeAsync<object>("alert","내용내용");
    4) prop + tab +tab 하면 프로퍼티(get, set)가 만들어짐
    5) invoke 시 해당 변수가 예(OnParentCall)
        OnParentCall?.invoke() ==> null 인지 확인 후 null 아니면 invoke 함수 실행

    6) 부모 (FrmChild.razor)
        @page "/Demos/ParentChild/FrmParent"
        @inject IJSRuntime js

        <h3>FrmParent</h3>
        <input type="button" value="부모에서 호출"
            @onclick="ParentCall"/>
        <hr />

        <FrmChild 
                FromParent="1234"
                OnParentCall="ParentCall"
                PageIndexChanged="PageIndexChanged"
                ></FrmChild>


        @code {
            protected void ParentCall()
            {
                js.InvokeAsync<object>("alert", "ParentCall호출됨");
            }

            protected void PageIndexChanged(int pageIndex)
            { 
                js.InvokeAsync<object>("alert", $"{pageIndex}인덱스 넘어옴");
            }
        }

    7) 자식 (FrmParent.razor)
        <h3>FrmChild</h3>
        @inject IJSRuntime js
        부모에서 전달된 값: @FromParent
        <input type="button" value="자식에서 호출"
            @onclick="btnChild_Click" />

        @code {
            [Parameter]
            public int FromParent { get; set; }

            [Parameter]
            public Action OnParentCall { get; set; }

            [Parameter]
            public Action<int> PageIndexChanged { get; set; }

            protected void btnChild_Click()
            {
                js.InvokeAsync<object>("alert", "btnChild_Click호출됨");
                OnParentCall?.Invoke(); // 부모에서 전송된 메서드 호출
            }

            protected void PagerButtonClicked(int pageNumber)
            {
                PageIndexChanged?.Invoke(pageNumber - 1); //null 확인 후 null이 아니면 invoke 함
            }
        }
        <input type="button" value="1페이지"
            @onclick="@(() => PagerButtonClicked(1))" />
        <input type="button" value="2페이지"
            @onclick="@(() => PagerButtonClicked(2))" />

4. SearchBox 중첨 컴포넌트_부모 컴포 넌트와 자식 컴포넌트 그리고 EventCallBack 대리자
    1) 소스에 변수와 바인드.
        @bind="SearchQuery" 
    2) 입력과 동시에 바인드 하는 방법(양방향 바인딩)
        @bind:event="oninput" 를 추가하여 입력과 동식에 바인딩됨
    3) 부모에서 설정값을 받아옴 IDictionary 형태로 받아옴
        자식 변수에 어노테이션에 CaptureUnmatchedValues = true 값을 해 줘야 함
        @attributes="AdditionalAttributes"
    4) 컴포넌트 속성은 값은 속성을 여러번 지정 할 경우 마지막(가장 오른쪽)의 값이 적용됨
    5) 자식 컴포넌트에서 발생한 정보를 부모 컴포넌트에게 전달
        [Parameter]
        public EventCallback<string> SearchQueryChanged { get; set; }
        Action 이랑 차이는 잘 모르겠음... 같은 거같은데

    6) 타이머 의 AutoReset 속성을 false로 주면 한번만 실행 하게함
        debounceTimer.AutoReset = false; //한번만 실행 하고 다시 실행 안하게함.
    7)  타이머는 사용 후 객체를 dispose를 해 줘야 해서 IDisposable를 상속 받아 타이머를 소멸 시킴
        @implements IDisposable 
        
        public void Dispose()
        {
            debounceTimer.Dispose();
        }

    8) 부모 (SearchBoxTest.razor)
        @page "/SearchBoxTest"
        @using BlazorApp.Components


        <h3>SearchBoxTest</h3>

        <SearchBox></SearchBox>
        <SearchBox placeholder="Search Query..." SearchQueryChanged="SearchQueryChanged"></SearchBox>

        <hr />
        부모: @searchQuery

        @code {
            private string searchQuery;
            protected void SearchQueryChanged(string searchQuery)
            {
                this.searchQuery = searchQuery; 
            }
        }

    9) SearchBox 컴포넌트 (SearchBox.razor)
        @using System.Timers
        @implements IDisposable 

        <div class="search">
            <i class="oi oi-eye"></i>
            <input placeholder="Search..."
                @attributes="AdditionalAttributes" @bind="SearchQuery" @bind:event="oninput" />
            <input type="button" value="Search" @onclick="Search" />
        </div>
        <hr />
        자식: @SearchQuery

        @code {
            private string searchQuery;
            private Timer debounceTimer;

            public string SearchQuery
            {
                get => searchQuery;
                set
                {
                    searchQuery = value;
                    debounceTimer.Stop();
                    debounceTimer.Start();
                }
            }

            [Parameter(CaptureUnmatchedValues = true)] //부모에서 넘겨주는 값을 다받아옴 CaptureUnmatchedValues 값이 true 일 경우
            public IDictionary<string, object> AdditionalAttributes { get; set; }

            // 자식 컴포넌트에서 발생한 정보를 부모 컴포넌트에게 전달
            [Parameter]
            public EventCallback<string> SearchQueryChanged { get; set; }

            [Parameter]
            public int Debounce { get; set; } = 300;

            protected override void OnInitialized()
            {
                debounceTimer = new Timer();
                debounceTimer.Interval = Debounce;
                debounceTimer.AutoReset = false; //한번만 실행 하고 다시 실행 안하게함.
                debounceTimer.Elapsed += SearchHandler;
            }

            protected void Search()
            {
                SearchQueryChanged.InvokeAsync(SearchQuery); // 부모의 메서드에 검색어 전달
            }

            protected async void SearchHandler(object source, ElapsedEventArgs e)
            {
                await InvokeAsync(() => SearchQueryChanged.InvokeAsync(SearchQuery)); // 부모의 메서드에 검색어 전달
            }

            public void Dispose()
            {
                debounceTimer.Dispose();
            }
        }
5. MatBlazor_머티리얼 디자인을 손쉽게 구현하는 MatBlazor 컴포넌트 소개
    1) Nuget패키지에서 MatBlazor 설치
    2) _Imports.razor에 추가
        @using MatBlazor
    3) _Host.cshtml에 head부분에 추가
        <script src="_content/MatBlazor/dist/matBlazor.js"></script>
    4) table을 사용하려면 Startup.cs 에 추가
        services.AddScoped<HttpClient>(); // MatBlazor
