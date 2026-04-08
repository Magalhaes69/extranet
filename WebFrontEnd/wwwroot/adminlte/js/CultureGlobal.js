window.blazorCulture = {
    get: () => window.localStorage['BlazorCulture'] || 'pt-PT',
    Set: (value) => window.localStorage['BlazorCulture'] = value
}