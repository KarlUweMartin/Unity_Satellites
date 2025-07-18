using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

public class TleClient
{
    private readonly HttpClient _httpClient;

    public TleClient(HttpClient httpClient = null)
    {
        _httpClient = httpClient ?? new HttpClient();
    }

    public async Task<string> DownloadTleAsync(string tleUrl)
    {
        if (string.IsNullOrWhiteSpace(tleUrl)) return null;

        try
        {
            var response = await _httpClient.GetAsync(tleUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Output.Instance.Text = ex.Message;
            throw;
        }
    }


    public Dictionary<string, string> DataSetUrls = new Dictionary<string, string>
    {
        { "Last30Days", BaseUrl + "gp.php?GROUP=last-30-days&FORMAT=tle" },
        { "Stations", BaseUrl + "gp.php?GROUP=stations&FORMAT=tle" },
        { "Visual", BaseUrl + "gp.php?GROUP=visual&FORMAT=tle" },
        { "Active", BaseUrl + "gp.php?GROUP=active&FORMAT=tle" },
        { "Analyst", BaseUrl + "gp.php?GROUP=analyst&FORMAT=tle" },
        { "Cosmos1408Debris", BaseUrl + "gp.php?GROUP=cosmos-1408-debris&FORMAT=tle" },
        { "Fengyun1CDebris", BaseUrl + "gp.php?GROUP=fengyun-1c-debris&FORMAT=tle" },
        { "Iridium33Debris", BaseUrl + "gp.php?GROUP=iridium-33-debris&FORMAT=tle" },
        { "Cosmos2251Debris", BaseUrl + "gp.php?GROUP=cosmos-2251-debris&FORMAT=tle" },
        { "Weather", BaseUrl + "gp.php?GROUP=weather&FORMAT=tle" },
        { "NOAA", BaseUrl + "gp.php?GROUP=noaa&FORMAT=tle" },
        { "GOES", BaseUrl + "gp.php?GROUP=goes&FORMAT=tle" },
        { "EarthResources", BaseUrl + "gp.php?GROUP=resource&FORMAT=tle" },
        { "SARSAT", BaseUrl + "gp.php?GROUP=sarsat&FORMAT=tle" },
        { "DisasterMonitoring", BaseUrl + "gp.php?GROUP=dmc&FORMAT=tle" },
        { "TDRSS", BaseUrl + "gp.php?GROUP=tdrss&FORMAT=tle" },
        { "ARGOS", BaseUrl + "gp.php?GROUP=argos&FORMAT=tle" },
        { "Planet", BaseUrl + "gp.php?GROUP=planet&FORMAT=tle" },
        { "Spire", BaseUrl + "gp.php?GROUP=spire&FORMAT=tle" },
        { "Geo", BaseUrl + "gp.php?GROUP=geo&FORMAT=tle" },
        { "GeoProtectedZone", BaseUrl + "gp.php?SPECIAL=gpz&FORMAT=tle" },
        { "GeoProtectedZonePlus", BaseUrl + "gp.php?SPECIAL=gpz-plus&FORMAT=tle" },
        { "Intelsat", BaseUrl + "gp.php?GROUP=intelsat&FORMAT=tle" },
        { "SES", BaseUrl + "gp.php?GROUP=ses&FORMAT=tle" },
        { "Eutelsat", BaseUrl + "gp.php?GROUP=eutelsat&FORMAT=tle" },
        { "Telesat", BaseUrl + "gp.php?GROUP=telesat&FORMAT=tle" },
        { "Starlink", BaseUrl + "gp.php?GROUP=starlink&FORMAT=tle" },
        { "OneWeb", BaseUrl + "gp.php?GROUP=oneweb&FORMAT=tle" },
        { "Qianfan", BaseUrl + "gp.php?GROUP=qianfan&FORMAT=tle" },
        { "Hulianwang", BaseUrl + "gp.php?GROUP=hulianwang&FORMAT=tle" },
        { "Kuiper", BaseUrl + "gp.php?GROUP=kuiper&FORMAT=tle" },
        { "IridiumNext", BaseUrl + "gp.php?GROUP=iridium-NEXT&FORMAT=tle" },
        { "Orbcomm", BaseUrl + "gp.php?GROUP=orbcomm&FORMAT=tle" },
        { "Globalstar", BaseUrl + "gp.php?GROUP=globalstar&FORMAT=tle" },
        { "Amateur", BaseUrl + "gp.php?GROUP=amateur&FORMAT=tle" },
        { "SatNOGS", BaseUrl + "gp.php?GROUP=satnogs&FORMAT=tle" },
        { "ExperimentalComm", BaseUrl + "gp.php?GROUP=x-comm&FORMAT=tle" },
        { "OtherComm", BaseUrl + "gp.php?GROUP=other-comm&FORMAT=tle" },
        { "GNSS", BaseUrl + "gp.php?GROUP=gnss&FORMAT=tle" },
        { "GPSOps", BaseUrl + "gp.php?GROUP=gps-ops&FORMAT=tle" },
        { "GLONASS", BaseUrl + "gp.php?GROUP=glo-ops&FORMAT=tle" },
        { "Galileo", BaseUrl + "gp.php?GROUP=galileo&FORMAT=tle" },
        { "Beidou", BaseUrl + "gp.php?GROUP=beidou&FORMAT=tle" },
        { "SBAS", BaseUrl + "gp.php?GROUP=sbas&FORMAT=tle" },
        { "NNSS", BaseUrl + "gp.php?GROUP=nnss&FORMAT=tle" },
        { "Musson", BaseUrl + "gp.php?GROUP=musson&FORMAT=tle" },
        { "Science", BaseUrl + "gp.php?GROUP=science&FORMAT=tle" },
        { "Geodetic", BaseUrl + "gp.php?GROUP=geodetic&FORMAT=tle" },
        { "Engineering", BaseUrl + "gp.php?GROUP=engineering&FORMAT=tle" },
        { "Education", BaseUrl + "gp.php?GROUP=education&FORMAT=tle" },
        { "Military", BaseUrl + "gp.php?GROUP=military&FORMAT=tle" },
        { "Radar", BaseUrl + "gp.php?GROUP=radar&FORMAT=tle" },
        { "Cubesat", BaseUrl + "gp.php?GROUP=cubesat&FORMAT=tle" },
        { "Other", BaseUrl + "gp.php?GROUP=other&FORMAT=tle" },
    };

    private const string BaseUrl = "https://celestrak.org/NORAD/elements/";

}