# TeeTimeBooker

## To-Do
- [x] Deployed to Azure and working
- [x] ConfigurationManager for PReferredTeeTimes and Smith Golf search settings
- [ ] Feature flag to enable booking or not
- [ ] Better error handling
- [ ] Separate 'requirements.txt' for each Azure Function (modify pipeline and tasks.json when they use requirements.txt for pip install): https://docs.microsoft.com/en-us/azure/azure-functions/functions-reference-python#folder-structure
- [x] Load AppSettings from KeyVault
- [x] Logging
- [ ] Break 'BookTeeTimes' into multiple functions
- [ ] Deploy AppSettings as ARM
- [ ] Local script that levergaes same Helper Classes
- [ ] Local and deployed instructions
- [x] Better way for preferred tee times
- [x] Dev & master branches
- [x] dev and prod environments & pipelines
- [ ] Better directory structure (functions, requirements, naming convention of classes/vars/functions/files)
- [x] Make Public repo
- [ ] GolfNow adapter??
- [x] Use APIs instead of Selenium
- [ ] Tests
- [ ] Ability to skip certain days/weeks/ranges
- [ ] Allow multiple people to enter info for bookings (GolfNowBooker is HttpTrigger by some other continuous job?)
- [ ] Use GitHub planner for roadmap
- [ ] Roadmap of different booking services to support (GolfNow, VTGolf, TeeOff, SupremeGolf,)
- [ ] Pull all user booking configurations from storage via timer trigger function, and enqueue with time delay on appropriate ServiceBus Topic Subscription for appropriate Booking workload azure function to consume
- [ ] Process workload requests in batch or scale out in parallel
- [ ] Expand to other booking stuff (Concerts, sports, etc.)
- [ ] Update ReadMe to include architecture and cleanup todo