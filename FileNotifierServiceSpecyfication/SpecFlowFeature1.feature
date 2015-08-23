Feature: FileNotifierFeature


@fileNotifier
Scenario: Set path into fileNotifier
	Given I have the fileNotifier service
	And I set path to observe
	When I create file
	Then notification was recived

@fileNotifier
Scenario: Remove path from fileNotifier
	Given I have the fileNotifier service
	And I set path to observe
	When I remove path to observe
	And I create file
	Then notification wasn't recived